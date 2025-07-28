using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlinkObject : MonoBehaviour
{
    public Component objectToBlink; // Assign any UI component here (Image, Text, Button, etc.)
    public float blinkDuration = 0.2f; // Time in seconds for each blink cycle (on/off)

    private bool isBlinking = false;

    void OnEnable()
    {
        if (objectToBlink != null)
        {
            StartBlinking();
        }
    }

    void OnDisable()
    {
        StopBlinking();
    }

    private void StartBlinking()
    {
        if (!isBlinking)
        {
            isBlinking = true;
            StartCoroutine(Blink());
        }
    }

    private void StopBlinking()
    {
        isBlinking = false;
        StopCoroutine(Blink());

        // Ensure the object is left in a visible/interactable state
        if (objectToBlink is Image image)
        {
            image.enabled = true;
        }
        else if (objectToBlink is Text text)
        {
            text.enabled = true;
        }
        else
        {
            objectToBlink.gameObject.SetActive(true);
        }
    }

    // start blinking
    private IEnumerator Blink()
    {
        while (isBlinking)
        {
            if (objectToBlink is Image image)
            {
                image.enabled = !image.enabled;
            }
            else if (objectToBlink is Text text)
            {
                text.enabled = !text.enabled;
            }
            else
            {
                objectToBlink.gameObject.SetActive(!objectToBlink.gameObject.activeSelf);
            }

            yield return new WaitForSeconds(blinkDuration);
        }
    }
}
