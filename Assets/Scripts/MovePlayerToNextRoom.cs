using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayerToNextRoom : MonoBehaviour
{
    public Vector3 positionToTP;

    static private GameController gameController;

    private void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // ManageTransitionCanvas does nothing at the moment --> it just does not let the player move for 0.35 seconds
            StartCoroutine(gameController.ManageTransitionCanvas());
            gameController.MovePlayerToNextRoom(positionToTP);
        }
    }
}
