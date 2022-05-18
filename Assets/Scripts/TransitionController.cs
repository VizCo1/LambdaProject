using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        yield return new WaitForSeconds(0.35f);
        blackImageCanvas.SetActive(false);
        playerMove.canMove = true;
    }
}
