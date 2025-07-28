using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(CanvasGroup))]
public class Fader : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;

    public IEnumerator FadeIn(float duration, bool skippable = false)
    {
        SetVisible(false);
        yield return StartCoroutine(FadeCanvasGroup(canvasGroup, canvasGroup.alpha, 1f, duration, skippable));
        SetVisible(true);
    }

    public IEnumerator FadeIn(float duration, float targetAlpha, bool skippable = false)
    {
        SetVisible(false);
        yield return StartCoroutine(FadeCanvasGroup(canvasGroup, canvasGroup.alpha, targetAlpha, duration, skippable));
    }

    public IEnumerator FadeOut(float duration, bool skippable = false)
    {
        SetVisible(true);
        yield return StartCoroutine(FadeCanvasGroup(canvasGroup, canvasGroup.alpha, 0f, duration, skippable));
        SetVisible(false);
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float startAlpha, float targetAlpha, float duration, bool skippable)
    {
        float startTime = Time.time;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = Mathf.Clamp01(elapsedTime / duration);
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            
            if (canvasGroup.alpha > 0f)
            {
                canvasGroup.blocksRaycasts = true;
            }

            elapsedTime = Time.time - startTime;

            // Check if the spacebar is pressed to skip the fade
            if (skippable)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    canvasGroup.alpha = targetAlpha; // Set alpha to the target value
                    break; // Exit the loop
                }
            }

            yield return null;
        }

        // Ensure the alpha is exactly the target value when the duration is complete
        canvasGroup.alpha = targetAlpha;
    }

    public void SetVisible(bool visible, float targetAlpha = -1)
    {
        StopAllCoroutines();

        if (targetAlpha == -1)
        {
            targetAlpha = visible ? 1 : 0;
        }

        if (visible)
        {
            canvasGroup.alpha = targetAlpha;
            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            canvasGroup.alpha = targetAlpha;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public bool GetVisible()
    {
        return canvasGroup.alpha > 0;
    }
}
