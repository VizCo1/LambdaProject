using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    private PlayerInputActions playerInputActions;
    private Rigidbody rb;

    private Vector2 inputVector;

    private Matrix4x4 isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));


    [SerializeField] private float movementSpeed;
    [SerializeField] private float turnSpeed;

    [SerializeField] private MeshRenderer mainModel;

    private bool dashIsReady;
    private bool canDash;
    public bool canMove;

    [SerializeField] private GameObject trails;

    private void Awake()
    {
        dashIsReady = true;
        canDash = false;
        canMove = true;

        rb = GetComponent<Rigidbody>();

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Movement.performed += ctx => { inputVector = ctx.ReadValue<Vector2>(); trails.SetActive(true); };
        playerInputActions.Player.Movement.canceled += ctx => { inputVector = Vector2.zero; trails.SetActive(false); };

        playerInputActions.Player.Dash.performed += ctx => { if (dashIsReady && inputVector != Vector2.zero) canDash = true; };
    }

    private void Update()
    {
        RotateMainModel();
    }

    private void FixedUpdate()
    {
        if (canMove) Move();

        if (canDash) Dash();
    }

    private void Move()
    {
        Vector3 isoInput = isoMatrix.MultiplyPoint3x4(new Vector3(inputVector.x, 0, inputVector.y));
        rb.MovePosition(transform.position + isoInput * movementSpeed  * Time.deltaTime);
    }

    private void RotateMainModel()
    {
        if (new Vector3(inputVector.x, 0, inputVector.y) != Vector3.zero)
        {
            Vector3 isoInput = isoMatrix.MultiplyPoint3x4(new Vector3(inputVector.x, 0, inputVector.y));
            Quaternion toRotation = Quaternion.LookRotation(isoInput, Vector3.up);
            transform.GetChild(0).rotation = Quaternion.RotateTowards(transform.GetChild(0).rotation, toRotation, turnSpeed * Time.deltaTime);
        }
    }

    public void Dash()
    {
        canDash = false;
        dashIsReady = false;
        rb.AddForce(transform.GetChild(0).forward * 250f, ForceMode.Force);
        StartCoroutine(DashTimer());
    }

    IEnumerator DashTimer()
    {
        yield return new WaitForSeconds(0.2f);
        Color basicColor = mainModel.material.color;
        Color newColor = Color.white;
        for(int i = 0; i < 4; i++)
        {
            mainModel.material.color = newColor;
            yield return new WaitForSeconds(0.15f);
            mainModel.material.color = basicColor;
            yield return new WaitForSeconds(0.05f);
        }

        dashIsReady = true;
    }

    public Vector3 GetInputVector()
    {
        return inputVector;
    }
}
