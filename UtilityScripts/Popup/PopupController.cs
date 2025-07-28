using System;
using System.Collections;
using UnityEngine;

public class PopupController : MonoBehaviour
{
    [Header("Base members")]
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] Transform foreground;
    [SerializeField] AudioClip shrinkClip;

    public Action PopupOpening, PopupClosing;

    private void OnEnable()
    {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        MakeVisible(false);
    }

    protected virtual void OnPopupOpened() {}
    protected virtual void OnPopupClosed() {}

    public void OpenPopup()
    {
        canvasGroup.interactable = true;
        MakeVisible(true);
        // enable content
        OnPopupOpened();
        PopupOpening?.Invoke();

        // play opening animation
        StartCoroutine(OpenAnimation());
    }

    public void OnClickClosePopup() // used to close via buttons that trigger shrinking sound
    {
        AudioManager.instance.PlaySound(shrinkClip);
        ClosePopup();
    }

    public void ClosePopup()
    {
        canvasGroup.interactable = false;
        StartCoroutine(CloseAnimation());
        PopupClosing?.Invoke();
        OnPopupClosed();
    }

    private void MakeVisible(bool visible)
    {
        if (visible)
        {
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;

        }
        else
        {
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
        }

    }

    protected IEnumerator Animate(bool enlarge)
    {
        float targetScale = enlarge ? 1f : 0.1f;
        float initialScale = enlarge ? 0.1f : 1f; // Set the initial scale opposite to the target

        foreground.localScale = new Vector3(initialScale, initialScale, 1);

        float duration = 0.1f; // Adjust the duration as needed

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float newScale = Mathf.Lerp(initialScale, targetScale, elapsedTime / duration);
            foreground.localScale = new Vector3(newScale, newScale, 1);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final scale is set to the target scale
        foreground.localScale = new Vector3(targetScale, targetScale, 1);

        // Add a yield return null here to wait for the frame to complete
        yield return null;
    }

    protected IEnumerator OpenAnimation()
    {
        yield return StartCoroutine(Animate(true));
    }

    protected IEnumerator CloseAnimation()
    {
        yield return StartCoroutine(Animate(false));
        MakeVisible(false);
    }

}
