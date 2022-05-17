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

    private Vector3 respawnPosition;

    private GameObject actualRoom;

    void Start()
    {
        playerMove = player.GetComponent<PlayerMove>();
        playerHealthStats = player.GetComponentInChildren<PlayerHealthStats>();
        //playerRb = player.GetComponent<Rigidbody>();
    }
    private void Update()
    {
        Debug.DrawLine(player.transform.position, player.transform.up, Color.black);
        if (actualRoom != null)
            Debug.Log(actualRoom.name);
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
        RaycastHit hit;
        if (Physics.Raycast(player.transform.position + Vector3.up* 4, Vector3.up, out hit, 40f))
        {
            //Debug.Log(hit.transform.tag);
            if (hit.transform.CompareTag("ActualRoomHelper"))
            {
                actualRoom = hit.transform.parent.gameObject;
            }
            else if (hit.transform.CompareTag("Level"))
            {
                actualRoom = hit.transform.parent.gameObject;
            }
        }
    }

    public void GetRespawnPosition(Vector3 position)
    {
        respawnPosition = position;
    }

    public void PlayerCannotMove()
    {
        playerMove.canMove = false;
    }

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
