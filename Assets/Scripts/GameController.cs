using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private Camera minimapCamera;

    [SerializeField]
    private GameObject player;

    private PlayerMove playerMove;
    private PlayerHealthStats playerHealthStats;
    //private Rigidbody playerRb;

    Vector3 respawnPosition;

    void Start()
    {
        playerMove = player.GetComponent<PlayerMove>();
        playerHealthStats = player.GetComponentInChildren<PlayerHealthStats>();
        //playerRb = player.GetComponent<Rigidbody>();
    }

    public IEnumerator ManageTransitionCanvas()
    {
        playerMove.canMove = false;
        //blackImageCanvas.SetActive(true);
        yield return new WaitForSeconds(0.35f);
        //blackImageCanvas.SetActive(false);    
        playerMove.canMove = true;
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

    public void GetRespawnPosition(Vector3 position)
    {
        respawnPosition = position;
    }

    public void PlayerCannotMove()
    {
        playerMove.canMove = false;
    }
    /*
    public void PushPlayer()
    {
        float force;
        // playerRb.gameObject.transform.position
        Vector3 dir = playerMove.GetInputVector();
        if (dir.magnitude < 0.2) force = 1500;
        else force = 750;
        Debug.Log("Force: " + force);
        playerRb.AddForce(dir * force);
    }
    */

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            player.transform.position = respawnPosition;
            playerHealthStats.TakeDamage(1);
            playerMove.canMove = true;
        }
    }
}
