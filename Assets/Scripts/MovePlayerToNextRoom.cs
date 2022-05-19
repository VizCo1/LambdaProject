using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MovePlayerToNextRoom : MonoBehaviour
{
    public Vector3 positionToTP;

    [SerializeField]
    private AudioSource teleportSource;
    public AudioSource portalSource;

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
            for (int i = 0; i < transform.parent.parent.childCount; i++)
            {
                GameObject portal = transform.parent.parent.GetChild(i).GetChild(0).gameObject;
                if (portal.activeSelf)
                {
                    portal.GetComponent<MovePlayerToNextRoom>().portalSource.Stop();
                }
            }
            teleportSource.Play();
            StartCoroutine(transitionController.ManageTransitionCanvas());
            gameController.MovePlayerToNextRoom(positionToTP);
        }
    }
}
