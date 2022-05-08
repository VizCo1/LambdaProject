using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    const float IMAGE_MAX_ALPHA = 0.5f;
    const float BORDER_MAX_ALPHA = 1f;

    [SerializeField]
    GameObject completeMinimap;

    RawImage completeMinimapImage;
    Image completeMinimapBorder;

    Color initialCompleteMinimapImageColor;
    Color initialCompleteMinimapBorderColor;

    private PlayerInputActions playerInputActions;
    void Start()
    {
        completeMinimapBorder = completeMinimap.transform.GetChild(1).gameObject.GetComponent<Image>();
        completeMinimapImage = completeMinimap.transform.GetChild(0).GetChild(0).gameObject.GetComponent<RawImage>();

        initialCompleteMinimapBorderColor = completeMinimapBorder.color;
        initialCompleteMinimapImageColor = completeMinimapImage.color;

        playerInputActions = new PlayerInputActions();
        playerInputActions.UI.Enable();

        playerInputActions.UI.ShowCompleteMinimap.performed += ctx => { RestartCompleteMinimapColors(); StopAllCoroutines(); ShowCompleteMinimap(); };
        playerInputActions.UI.ShowCompleteMinimap.canceled += ctx => { StopAllCoroutines(); HideCompleteMinimap(); };
    }

    void ShowCompleteMinimap()
    {
        StartCoroutine(IncreaseImageAlpha());
        StartCoroutine(IncreaseBorderAlpha());
    }

    void HideCompleteMinimap()
    {
        StartCoroutine(DecreaseImageAlpha());
        StartCoroutine(DecreaseBorderAlpha());
        completeMinimapImage.color = initialCompleteMinimapImageColor;
        completeMinimapBorder.color = initialCompleteMinimapBorderColor;
    }

    IEnumerator IncreaseImageAlpha()
    {
        Color color = initialCompleteMinimapImageColor;
        while (color.a < IMAGE_MAX_ALPHA)
        {
            color.a += 0.05f;
            completeMinimapImage.color = color;
            yield return new WaitForSeconds(0.025f);
        }
    }

    IEnumerator IncreaseBorderAlpha()
    {
        Color color = initialCompleteMinimapBorderColor;
        while (color.a < BORDER_MAX_ALPHA)
        {
            color.a += 0.05f;
            completeMinimapBorder.color = color;
            yield return new WaitForSeconds(0.025f);
        }
    }
    IEnumerator DecreaseImageAlpha()
    {
        Color color = completeMinimapImage.color;
        while (color.a > 0)
        {
            color.a -= 0.05f;
            completeMinimapImage.color = color;
            yield return new WaitForSeconds(0.015f);
        }
    }

    IEnumerator DecreaseBorderAlpha()
    {
        Color color = completeMinimapBorder.color;
        while (color.a > 0)
        {
            color.a -= 0.1f;
            completeMinimapBorder.color = color;
            yield return new WaitForSeconds(0.015f);
        }
    }

    void RestartCompleteMinimapColors()
    {
        completeMinimapBorder.color = initialCompleteMinimapBorderColor;
        completeMinimapImage.color = initialCompleteMinimapImageColor;
    }
}
