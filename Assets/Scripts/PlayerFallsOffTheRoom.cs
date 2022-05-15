using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallsOffTheRoom : MonoBehaviour
{
    static private GameController gameController;

    private void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            Debug.Log("PlayerFallsOffTheRoom");
            gameController.PlayerCannotMove();
            //gameController.PushPlayer();
        }
    }
}
