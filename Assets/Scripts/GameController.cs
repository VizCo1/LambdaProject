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

    void Start()
    {
        playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
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
}
