using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionController : MonoBehaviour
{
    [SerializeField]
    private PlayerMove playerMove;
    public GameObject blackImageCanvas;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator ManageTransitionCanvas()
    {
        playerMove.canMove = false;
        blackImageCanvas.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        blackImageCanvas.SetActive(false);
        playerMove.canMove = true;
    }

    public IEnumerator ManageTransitionCanvasGameOver()
    {
        blackImageCanvas.SetActive(true);
        playerMove.canMove = false;

        yield return new WaitForSeconds(3);

        SceneManager.LoadSceneAsync(2);

        yield return null;
    }
}
