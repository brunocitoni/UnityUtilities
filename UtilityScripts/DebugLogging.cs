using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugLogging : MonoBehaviour
{
    public static TMP_Text debugText; // Reference to your TMP object
    [SerializeField] private static int maxCharsOnScreen = 100000;

    public static void AssignTextElement(TMP_Text _debugText)
    {
        debugText = _debugText;
    }

    public static void Log(string message)
    {
        Debug.Log(message);
    }

    public static void LogMessage(string message)
    {
        if (Utilities.IsOnMainThread())
        {
            if (debugText != null)
            {
                if (debugText.text.Length > maxCharsOnScreen)
                {
                    ClearLog();
                }
                debugText.text += "-" + message + "\n";
                Debug.Log(message);
            }
        }
    }

    public static void LogMessage<T>(List<T> List)
    {
        if (Utilities.IsOnMainThread())
        {
            if (debugText != null)
            {
                if (debugText.text.Length > maxCharsOnScreen)
                {
                    ClearLog();
                }
                foreach (T t in List)
                {
                    debugText.text += t + "\n";
                    Debug.Log(List + "\n");
                }
            }
        }
    }

    public static void ClearLog()
    {
        if (Utilities.IsOnMainThread())
        {
            if (debugText != null)
            {
                debugText.text = "";
            }
        }
    }
}

