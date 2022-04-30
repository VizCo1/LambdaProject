using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    const int PORTALS_OBJECT = 1;
    const int NUMBER_OF_DOORS = 4;

    [SerializeField]
    private GameObject roomMinimap;
    [SerializeField]
    private GameObject lineDoor;

    [SerializeField]
    private Transform completeMinimapCamera;


    void Update()
    {
        
    }
    public void CreateMinimap(Transform dungeonGenerator)
    {
        int n = dungeonGenerator.transform.childCount;

        for (int i = 0; i < n; i++)
        {
            Transform room = dungeonGenerator.GetChild(i);
            Vector3 roomPosition = room.position + new Vector3(0, 0.5f, 0);


            var minimapRoom = Instantiate(roomMinimap, roomPosition, Quaternion.identity, transform);
            minimapRoom.name += ": " + i;
 
            InstantiateHallways(NUMBER_OF_DOORS, room, i+2);
        }

        MoveCompleteMinimapCameraToGivenPos(this.gameObject);
    }
    
    void InstantiateHallways(int n, Transform room, int roomIndex)
    {
        bool[] doors = new bool[n];
        for (int i = 0; i < doors.Length; i++)
        {
            if (doors[i] = room.GetChild(PORTALS_OBJECT).GetChild(i).gameObject.activeSelf)
            {
                Vector3[] data = LineDirectionAndOffset(i);
                Instantiate(lineDoor, room.position + data[0], Quaternion.Euler(data[1]), transform.GetChild(roomIndex));
            }
        }
    }

    const int LINE_OFFSET = 8;
    const int LINE_VERTICAL_OFFSET = -5;
    Vector3[] LineDirectionAndOffset(int i)
    {
        Vector3 direction = Vector3.zero;
        Vector3 position;

        if (i == 0)
        {
            direction = new Vector3(0, -90, 0);
            position = new Vector3(0, LINE_VERTICAL_OFFSET, LINE_OFFSET);
        }
        else if (i == 1)
        {
            direction = new Vector3(0, 90, 0);
            position = new Vector3(0, LINE_VERTICAL_OFFSET, -LINE_OFFSET);
        }
        else if (i == 2)
        {
            position = new Vector3(LINE_OFFSET, LINE_VERTICAL_OFFSET, 0);
        }
        else
        {
            position = new Vector3(-LINE_OFFSET, LINE_VERTICAL_OFFSET, 0);
        }

        return new Vector3[] { position, direction };
    }

    void MoveCompleteMinimapCameraToGivenPos(GameObject parentObject)
    {
        Vector3 sumVector = new Vector3(0f, 0f, 0f);

        foreach (Transform child in parentObject.transform)
        {
            sumVector += child.position;
        }

        Vector3 groupCenter = sumVector / parentObject.transform.childCount;

        completeMinimapCamera.position = new Vector3 (groupCenter.x, 10, groupCenter.z);
    }
}
