using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    //public GameObject[] walls; // 0 - Up 1 -Down 2 - Right 3- Left
    public GameObject[] doors;
    /* 0 - Up       -> Down
     * 1 - Down     -> Up
     * 2 - Right    -> Left
     * 3 - Left     -> Right
    */
    public MovePlayerToNextRoom[] portals;

    public void ConnectDoors(Vector3[] positionsToTP)
    {
        for (int i = 0; i < portals.Length; i++)
        {
            if (positionsToTP[i] != null)
            {
                portals[i].positionToTP = positionsToTP[i];
            }
        }
    }

    public void UpdateRoom(bool[] status)
    {
        for (int i = 0; i < status.Length; i++)
        {
            doors[i].SetActive(status[i]);
            //walls[i].SetActive(!status[i]);
        }
    }
}
