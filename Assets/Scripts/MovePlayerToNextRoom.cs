using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayerToNextRoom : MonoBehaviour
{
    public Vector3 positionToTP;

    static private TransitionController transitionController;
    static private GameController gameController;

    private void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        transitionController = GameObject.FindGameObjectWithTag("TransitionController").GetComponent<TransitionController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(transitionController.ManageTransitionCanvas());
            gameController.MovePlayerToNextRoom(positionToTP);
        }
    }
}
