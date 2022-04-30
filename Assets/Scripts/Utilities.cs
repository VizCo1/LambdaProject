using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities  
{
    const int PORTALS_OBJECT = 1;

    public static void InstantiateHallways(int n, Transform room)
    {
        bool[] doors = new bool[n];
        for (int i = 0; i < doors.Length; i++)
        {
            doors[i] = room.GetChild(PORTALS_OBJECT).GetChild(i).gameObject.activeSelf;
        }
    }
}
