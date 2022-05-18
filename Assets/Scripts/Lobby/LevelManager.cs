using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private TransitionController transitionController;

    [SerializeField]
    private bool isLevelScene;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            transitionController.blackImageCanvas.SetActive(true);
            if (isLevelScene)
                SceneManager.LoadSceneAsync("Scenes/GameScenes/Lobby");
            else
                SceneManager.LoadSceneAsync("Scenes/GameScenes/Level");
        }
    }
}
