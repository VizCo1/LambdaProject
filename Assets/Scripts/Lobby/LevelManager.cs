using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    [SerializeField]
    TransitionController transitionController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            transitionController.blackImageCanvas.SetActive(true);
            SceneManager.LoadSceneAsync("Level");
        }
    }
}
