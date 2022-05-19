using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Transform menuCamera;

    [SerializeField]
    private Transform playButton;
    [SerializeField]
    private Transform exitButton;

    [SerializeField]
    private AudioSource buttonAudio;
    [SerializeField]
    private AudioSource music;

    private bool playButtonPressed;
    private bool exitButtonPressed;
    private bool exitButtonMoved;
    private bool playButtonMoved;
    private bool cameraMoved;

    private void Update()
    {
        if (playButtonPressed)
        {
            StartCoroutine(MovePlayButton());
            StartCoroutine(MoveExitButton());
            StartCoroutine(MoveCamera());

            if (playButtonMoved && exitButtonMoved && cameraMoved)
            {
                music.Stop();
                SceneManager.LoadSceneAsync(1);
            }
        }
        else if (exitButtonPressed)
        {
            StartCoroutine(MoveExitButton());

            if (exitButtonMoved) Application.Quit();
        }
    }

    public void PlayButtonPressed()
    {
        if (!exitButtonPressed)
        {
            playButtonPressed = true;
            buttonAudio.Play();
        }
    }

    public void ExitButtonPressed()
    {
        if (!playButtonPressed)
        {
            exitButtonPressed = true;
            buttonAudio.Play();
        }
    }

    private IEnumerator MoveExitButton()
    {
        while (exitButton.position.x < 5000)
        {
            exitButton.position += new Vector3(2, 0, 0);
            yield return new WaitForSeconds(0.05f);
        }

        exitButtonMoved = true;
    }

    private IEnumerator MovePlayButton()
    {
        while (playButton.position.x < 5000)
        {
            playButton.position += new Vector3(2, 0, 0);
            yield return new WaitForSeconds(0.05f);
        }

        playButtonMoved = true;
    }

    private IEnumerator MoveCamera()
    {
        while (menuCamera.position.z < 650)
        {
            menuCamera.position += new Vector3(0, 0, 0.25F);
            yield return new WaitForSeconds(0.1f);
        }

        cameraMoved = true;
    }
} 
