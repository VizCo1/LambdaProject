using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomStatus : MonoBehaviour
{
    [SerializeField]
    private GameObject[] enemies;
    int enemyPool;

    [SerializeField]
    private GameObject portalPlatforms;
    public float limit;

    static private GameController gameController;

    private bool doorsAreDown;

    void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        doorsAreDown = true;
        //isRoomCompleted = false;

        if (enemies.Length == 0)
        {
            Debug.Log("No hay enemigos");
            doorsAreDown = false;
            StartCoroutine(ElevateDoors(limit));
            gameController.SetRespawnPosition(new Vector3(0, 0.5f, 0));
        }
        else 
        {
            enemyPool = Random.Range(0, enemies.Length);
        }
    }

    private void Update()
    {
        if (doorsAreDown && enemies[enemyPool].transform.childCount == 0 )
        {
            Debug.Log("You killed every enemy");
            doorsAreDown = false;
            gameController.SetRespawnPosition(transform.position + new Vector3(0, 0.5f, 0));
            StartCoroutine(ElevateDoors(limit));
        }
    }

    IEnumerator ElevateDoors(float limit)
    { 
        while (portalPlatforms.transform.position.y < limit) 
        {
            portalPlatforms.transform.position = Vector3.MoveTowards(portalPlatforms.transform.position, 
                new Vector3(portalPlatforms.transform.position.x, limit, portalPlatforms.transform.position.z), 8f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        for (int i = 0; i < portalPlatforms.transform.childCount; i++)
        {
            GameObject portalPlatform = portalPlatforms.transform.GetChild(i).gameObject;
            if (portalPlatform.activeSelf)
            {
                portalPlatform.transform.GetChild(0).gameObject.SetActive(true);
            }
        }

        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (enemies.Length == 0 || enemies[enemyPool].transform.childCount == 0)
            {
                gameController.SetRespawnPosition(transform.position + new Vector3(0, 0.5f, 0));
                gameController.gameObject.transform.position = new Vector3(transform.position.x, gameController.transform.position.y, transform.position.z);
            }
            else
            {
                enemies[enemyPool].SetActive(true);
                gameController.gameObject.transform.position = new Vector3(transform.position.x, gameController.transform.position.y, transform.position.z);
                gameController.actualRoom = this.gameObject;
                gameController.enemyGroupNumber = enemyPool;
            }
        }
    }
}
