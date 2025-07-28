using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TypewriterEffect : MonoBehaviour
{
    [SerializeField] private float standardDelay = 0.05f; // Default typing speed for standard letters
    [SerializeField] private float longerDelay = 0.1f; // Delay after a comma
    [SerializeField] private float longestDelay = 0.3f; // Delay after a period
    [SerializeField] private List<AudioClip> keySounds;

    private TMP_Text m_text;
    private string textToType;
    public bool done = false;
    private bool typing = false;

    Coroutine typingCoroutine;

    public void Initialise(TMP_Text text)
    {
        m_text = text;
    }

    private IEnumerator TypeText()
    {
        done = false;
        m_text.text = ""; // Clear the text initially
        string currentText = "";
        Stack<string> openTags = new Stack<string>(); // Track open tags

        for (int i = 0; i < textToType.Length; i++)
        {
            char character = textToType[i];

            if (character == '<') // Start of a tag
            {
                int closingIndex = textToType.IndexOf('>', i); // Find the end of the tag
                if (closingIndex != -1)
                {
                    string tag = textToType.Substring(i, closingIndex - i + 1); // Extract the full tag
                    currentText += tag;

                    // Check if it's an opening tag (e.g., <i>) or closing tag (e.g., </i>)
                    if (!tag.StartsWith("</"))
                    {
                        openTags.Push(tag); // Push the opening tag onto the stack
                    }
                    else if (openTags.Count > 0)
                    {
                        openTags.Pop(); // Pop the corresponding opening tag
                    }

                    i = closingIndex; // Skip to the end of the tag
                }
            }
            else
            {
                currentText += character;

                // Add open tags to maintain formatting
                string fullText = currentText;
                foreach (var openTag in openTags)
                {
                    fullText += "</" + openTag.Substring(1); // Add the corresponding closing tag
                }

                m_text.text = fullText; // Update the displayed text

                // Play key sound
                if (keySounds != null && keySounds.Count > 0)
                {
                    int index = Random.Range(0, keySounds.Count);
                    AudioManager.instance.PlaySound(keySounds[index], default, new Vector2(0.8f, 1.2f));
                }

                // Determine the delay based on the character type
                float delay = standardDelay;
                if (character == '.' || character == '!' || character == '?')
                {
                    delay = longestDelay; // Longer delay for periods
                }
                else if (character == ',' || character == ';')
                {
                    delay = longerDelay; // Slightly longer delay for commas
                }

                yield return new WaitForSeconds(delay); // Wait for the calculated delay
                typing = true;
            }
        }
        done = true;
        typing = false;
    }

    public void StartTyping(string newText)
    {
        textToType = newText;

        typingCoroutine = StartCoroutine(TypeText());
    }

    private void Update()
    {
        if (typing)
        {
            if (Input.GetMouseButtonDown(0))
            {
                StopCoroutine(typingCoroutine);
                m_text.text = textToType; // Immediately display the full text
                typing = false;
                done = true;
            }
        }
    }

}
