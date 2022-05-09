using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomStatus : MonoBehaviour
{
    //[SerializeField]
    //Enemy[] enemies;

    [SerializeField]
    GameObject portalPlatforms;
    public float limit;

    int numberOfEnemies;

    bool isRoomCompleted;

    void Awake()
    {
        isRoomCompleted = false;
        //numberOfEnemies = enemies.Length;
        numberOfEnemies = 0;

        if (numberOfEnemies == 0) StartCoroutine(ElevateDoors(limit)); 
    }

    void CheckEnemiesLeft()
    {
        
    }

    IEnumerator ElevateDoors(float limit)
    { 
        while (portalPlatforms.transform.position.y < limit) 
        {
            portalPlatforms.transform.position = Vector3.MoveTowards(portalPlatforms.transform.position, 
                new Vector3(portalPlatforms.transform.position.x, limit, portalPlatforms.transform.position.z), 4f * Time.deltaTime);
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

    // Cuando se muere un enemigo se llama a una funcion
}
