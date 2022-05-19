using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private AudioSource music01;
    [SerializeField]
    private AudioSource music02;

    [SerializeField]
    private AudioSource gameOverSound;
    [SerializeField]
    private AudioSource playerFallsSound;

    [SerializeField]
    private Camera minimapCamera;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private TransitionController transitionController;

    [SerializeField]
    private GameObject portal;

    private PlayerMove playerMove;
    private PlayerHealthStats playerHealthStats;

    private Vector3 respawnPosition;

    public GameObject actualRoom;
    public int enemyGroupNumber;

    void Start()
    {
        playerMove = player.GetComponent<PlayerMove>();
        playerHealthStats = player.GetComponentInChildren<PlayerHealthStats>();
    }

    public void MoveMinimapCamera(Vector3 nextRoomPosition)
    {
        Vector3 offset = new Vector3(7.5f, 10, -7.5f);
        Vector3 finalPos = nextRoomPosition + offset;
        minimapCamera.transform.position = finalPos;
    }

    public void MovePlayerToNextRoom(Vector3 nextPosition)
    {
        player.transform.position = nextPosition;
    }

    public void SetRespawnPosition(Vector3 position)
    {
        respawnPosition = position;
    }

    public void PlayerCannotMove()
    {
        playerMove.canMove = false;
    }

    public void MovePortalToPos(Vector3 pos)
    {
        portal.transform.position = pos;
        portal.SetActive(true);
    }

    public void MusicToBossDefeated()
    {
        music01.Stop();
        music02.Play();
    }

    public void GameOver()
    {
        Debug.Log(actualRoom.name);
        if (actualRoom != null)
        {
            for (int i = 0; i < actualRoom.transform.GetChild(1).childCount; i++)
            {
                GameObject portal = actualRoom.transform.GetChild(1).GetChild(i).GetChild(0).gameObject;
                if (portal.activeSelf)
                    portal.GetComponent<MovePlayerToNextRoom>().portalSource.Stop();
            }
        }

        gameOverSound.Play();
        StartCoroutine(transitionController.ManageTransitionCanvasGameOver());
        music01.Stop();
        music02.Stop();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            player.transform.position = respawnPosition;
            playerHealthStats.TakeDamage(1);

            if (playerHealthStats.Health <= 0)
            {
                GameOver();
                return;
            }
            else
            {
                playerFallsSound.Play();
            }

            StartCoroutine(transitionController.ManageTransitionCanvas());

            if (actualRoom != null)
            {
                Transform activeGroupOfEnemies = actualRoom.transform.GetChild(actualRoom.transform.childCount - 1).gameObject.transform.GetChild(enemyGroupNumber);

                if (activeGroupOfEnemies.childCount != 0)
                {
                    for (int i = 0; i < activeGroupOfEnemies.childCount; i++)
                    {
                        Enemy enemy = activeGroupOfEnemies.GetChild(i).GetComponentInChildren<Enemy>();
                        enemy.ResetEnemy();
                        activeGroupOfEnemies.gameObject.SetActive(false);
                    }
                }
            }

            playerMove.canMove = true;
        }
    }
}
