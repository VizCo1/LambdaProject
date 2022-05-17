using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMinimap : MonoBehaviour
{
    static private GameController gameController;

    void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameController.MoveMinimapCamera(transform.position);
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
        }
    }
}
