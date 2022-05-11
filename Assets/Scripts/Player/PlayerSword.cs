using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSword : MonoBehaviour
{
    const int MIN_SPEED = 500;
    const int MAX_SPEED = 750;

    float timeStart = 0; 

    private PlayerInputActions playerInputActions;

    [SerializeField] private Transform player;
    [SerializeField] private TrailRenderer[] trailRenderers;
    [SerializeField] private GameObject[] swords;

    private bool leftRotation;
    private bool canRotate;
    private bool scaleCompleted;
    [SerializeField] private bool twoSwords;

    private Vector3 initialScale;

    private Color redColor;
    private Color blueColor;

    [SerializeField]
    private Material redGlow;
    [SerializeField]
    private Material blueGlow;

    void Awake()
    {
        redColor = Color.red;
        blueColor = Color.blue;

        scaleCompleted = false;
        initialScale = transform.localScale;

        //rb = GetComponent<Rigidbody>();

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();


        playerInputActions.Player.RotateLeft.performed += ctx => { RotationInputPerformed(true); ChangeSwordsColor(redGlow, redColor); };
        playerInputActions.Player.RotateLeft.canceled += ctx => { if (leftRotation) { RotationInputCanceled(); }  };

        playerInputActions.Player.RotateRight.performed += ctx => {  RotationInputPerformed(false); ChangeSwordsColor(blueGlow, blueColor); };
        playerInputActions.Player.RotateRight.canceled += ctx => { if (!leftRotation) { RotationInputCanceled(); } };

        gameObject.SetActive(false);
    }

    void Update()
    {
        if (!scaleCompleted && canRotate)
        {
            timeStart += Time.deltaTime;
            SpawnSword(timeStart / 1f);
        }
        if (canRotate)
        {
            timeStart += Time.deltaTime;
            Yrotation(timeStart / 2f);
        }
        else if (!canRotate && gameObject.activeSelf)
        {
            timeStart += Time.deltaTime;
            DespawnSword(timeStart / 1f);
        }
    }

    void Yrotation(float t)
    {
        Vector3 vector3 = new (0, 1, 0);
        if (leftRotation)
        {
            vector3 = -vector3;
        }

        float yRotationSpeed = Mathf.Lerp(MIN_SPEED, MAX_SPEED, t);
        transform.RotateAround(player.position, vector3, (yRotationSpeed * Time.deltaTime));
    }

    void SpawnSword(float t)
    {
        swords[0].transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0.8f, 0.8f, 0.8f), t);

        if (twoSwords) swords[1].transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0.8f, 0.8f, 0.8f), t);

        if (transform.localScale == new Vector3(0.8f, 0.8f, 0.8f) && !scaleCompleted)
        {
            scaleCompleted = true;
            timeStart = 0;
        }
    }

    void DespawnSword(float t)
    {
        swords[0].transform.localScale = Vector3.Lerp(transform.localScale, initialScale, t);

        if (twoSwords) swords[1].transform.localScale = Vector3.Lerp(transform.localScale, initialScale, t);

        if (transform.localScale.magnitude <= initialScale.magnitude * 1.1f)
        {
            RestartSwords();
        }
    }

    void ChangeSwordsColor(Material glow, Color color)
    {

        for (int i = 0; i < swords.Length; i++)
        {
            swords[i].transform.GetChild(3).gameObject.GetComponent<MeshRenderer>().material = glow;
            for (int j = 0; j < swords[i].transform.childCount - 1; j++)
            {
                swords[i].transform.GetChild(j).gameObject.GetComponent<TrailRenderer>().startColor = color;
            }
        }
    }

    void RestartSwords()
    {
        scaleCompleted = false;
        gameObject.SetActive(false);
    }

    void RotationInputPerformed(bool direction)
    {
        leftRotation = direction;
        gameObject.SetActive(true);
        canRotate = true; 
        scaleCompleted = false;
    }

    void RotationInputCanceled()
    {
        canRotate = false;
        timeStart = 0; 
    }

    public void TwoSwordsSkill()
    {
        if (!twoSwords)
        {
            twoSwords = true;
            swords[1].SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
