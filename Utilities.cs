/*
 * Utilities.cs
 * 
 * Utilities functions accomplishing various tasks that are commonly requested in any project
 * 
 * Author: Bruno Citoni
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;

public static class Utilities
{
    public static string GenerateRandomKey()
    {
        return Guid.NewGuid().ToString();
    }

    public static bool IsOnMainThread()
    {
        return Thread.CurrentThread.ManagedThreadId == 1;
    }

    /// <summary>
    /// Returns true with a chance of desideredProbability%
    /// </summary>
    /// <param name="desideredProbability"> Probability out of a 100 to return true</param>
    /// <returns></returns>
    public static bool PercentageRoll(int desideredProbability)
    {
        int rand = UnityEngine.Random.Range(0, 100);
        if (rand <= desideredProbability)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Compares List<List<> content
    /// </summary>
    public static bool AreListOfListsEqual<T>(List<List<T>> list1, List<List<T>> list2)
    {
        if (list1.Count != list2.Count)
        {
            return false; // If the outer lists have different lengths, they can't be equal.
        }

        for (int i = 0; i < list1.Count; i++)
        {
            if (!list1[i].SequenceEqual(list2[i]))
            {
                return false; // If any inner list is not equal, the whole structure is not equal.
            }
        }

        return true; // Both outer and inner lists are equal.
    }


    public static bool ContainsString(List<List<string>> nestedList, string searchString)
    {
        foreach (var list in nestedList)
        {
            foreach (var item in list)
            {
                if (item == searchString)
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Shuffles a list
    /// </summary>
    public static void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }


    public static T PickAndRemove<T>(List<T> list)
    {
        if (list.Count == 0)
        {
            throw new InvalidOperationException("The list is empty.");
        }

        // Pick the first element from the list
        T pickedValue = list[0];

        // Remove the picked element from the list
        list.RemoveAt(0);

        return pickedValue;
    }

    /// <summary>
    /// Return a random element selected from the first X of an input list
    /// </summary>
    public static T GetRandomValueFromFirstX<T>(List<T> list, int x)
    {
        if (list == null || list.Count == 0 || x <= 0)
            throw new ArgumentException("Invalid input parameters");

        // Ensure x does not exceed the size of the list
        x = Math.Min(x, list.Count);

        int randomIndex = UnityEngine.Random.Range(0, x); // Generate a random index between 0 (inclusive) and x (exclusive)

        return list[randomIndex];
    }

    /// <summary>
    /// Prints a 2d array to console and optionally to file
    /// </summary>
    public static void Print2DArray<T>(T[,] inputArray, string textFilePath = null)
    {
        string toprint = "";
        for (int y = 0; y < inputArray.GetLength(1); y++)
        {
            for (int x = 0; x < inputArray.GetLength(0); x++)
            {
                toprint = toprint + inputArray[x, y] + "\t";
            }
            toprint += "\n";
        }
        Debug.Log(toprint);

        if (textFilePath != null)
        {
            StreamWriter writer = new StreamWriter(textFilePath, true);
            writer.Write(toprint + "\n\n");
            writer.Close();
        }
    }

    /// <summary>
    /// Prints a List to console and optionally to file
    /// </summary>
    public static void PrintList<T>(List<T> inputList, string textFilePath = null, bool append = true)
    {
        string toprint = "";
        for (int y = 0; y < inputList.Count; y++)
        {
            toprint += inputList[y] + "\n";
        }
        Debug.Log(toprint + "\n");
        if (textFilePath != null)
        {
            using (StreamWriter writer = new StreamWriter(textFilePath, append))
            {
                writer.Write(toprint + "\n\n");
            }
        }
    }

    /// <summary>
    /// Insert an element in a list in its sorted position
    /// </summary>
    public static void InsertSorted<T>(List<T> sortedList, T itemToInsert)
    {
        int index = sortedList.BinarySearch(itemToInsert);
        if (index < 0)
        {
            // The element doesn't exist in the list; insert it
            index = ~index; // Get the bitwise complement to find the correct insertion point
        }

        sortedList.Insert(index, itemToInsert);
    }

    /// <summary>
    /// Get lat N elements of a List
    /// </summary>
    public static List<T> GetLastNElements<T>(List<T> originalList, int N)
    {
        // Check if N is greater than the list size or the list is empty
        if (N >= originalList.Count || originalList.Count == 0)
        {
            // If N is greater or equal, return the entire list
            return originalList;
        }

        // Use GetRange to get the last N elements
        int startIndex = originalList.Count - N;
        List<T> lastNElements = originalList.GetRange(startIndex, N);

        return lastNElements;
    }


    public static int GetRandomWeightedIndex(float[] weights)
    {
        if (weights == null || weights.Length == 0) return -1;

        float w;
        float t = 0;
        int i;
        for (i = 0; i < weights.Length; i++)
        {
            w = weights[i];

            if (float.IsPositiveInfinity(w))
            {
                return i;
            }
            else if (w >= 0f && !float.IsNaN(w))
            {
                t += weights[i];
            }
        }

        float r = UnityEngine.Random.value;
        float s = 0f;

        for (i = 0; i < weights.Length; i++)
        {
            w = weights[i];
            if (float.IsNaN(w) || w <= 0f) continue;

            s += w / t;
            if (s >= r) return i;
        }

        return -1;
    }

    // find an inactive game object by name
    public static GameObject FindInactiveObjectByName(string name)
    {
        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].hideFlags == HideFlags.None)
            {
                if (objs[i].name == name)
                {
                    return objs[i].gameObject;
                }
            }
        }
        return null;
    }

    public static int GenerateRandomIntSeed(string seed)
    {
        return seed.GetHashCode() * Application.identifier.GetHashCode();
    }

    public static GameObject FindParentWithName(GameObject childObject, string name)
    {
        Transform t = childObject.transform;
        while (t.parent != null)
        {
            if (t.parent.name == name)
            {
                return t.parent.gameObject;
            }
            t = t.parent.transform;
        }
        return null; // Could not find a parent with given tag.
    }

    public static void DestroyAllChildren(GameObject go)
    {
        // Iterate through all child objects
        foreach (Transform child in go.transform)
        {
            // Destroy the child GameObject
            GameObject.Destroy(child.gameObject);
        }
    }

    public static string DecimalToHexadecimal(int decimalNumber)
    {
        return decimalNumber.ToString("X");
    }

    public static string HexadecimalToDecimal(string hexadecimalNumber, bool pad, int padLenght)
    {
        if (hexadecimalNumber.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
        {
            // Remove the "0x" prefix if it exists
            hexadecimalNumber = hexadecimalNumber.Substring(2);
        }

        int decimalValue = Convert.ToInt32(hexadecimalNumber, 16);

        if (pad)
        {
            // Convert the decimal value to a string and pad it to a total length of padLenght digits
            return decimalValue.ToString("D"+ padLenght.ToString());
        }

        return decimalValue.ToString();
    }

    public static T FindComponentInChildren<T>(GameObject parent) where T : Component
    {
        T component = parent.GetComponent<T>();
        if (component != null)
        {
            return component;
        }

        for (int i = 0; i < parent.transform.childCount; i++)
        {
            component = FindComponentInChildren<T>(parent.transform.GetChild(i).gameObject);
            if (component != null)
            {
                return component;
            }
        }

        return null;
    }

    public static int Factorial(int x)
    {
        int fact = 1;
        for (int i = 1; i <= x; i++)
        {
            fact *= i;
        }
        return fact;
    }

    public static long GetCurrentUnixTimestamp()
    {
        DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        TimeSpan timeSinceEpoch = DateTime.UtcNow - unixEpoch;
        return (long)timeSinceEpoch.TotalSeconds;
    }

    public static int CountTotalElements<T>(List<List<T>> listOfLists)
    {
        int totalElementCount = 0;

        foreach (List<T> innerList in listOfLists)
        {
            totalElementCount += innerList.Count;
        }

        return totalElementCount;
    }

    public static string PadLeftToLength(int number, int desiredLength)
    {
        return PadLeftToLength(number.ToString(), desiredLength);
    }

    public static string PadLeftToLength(string input, int desiredLength)
    {
        if (input.Length >= desiredLength)
        {
            // If the input is already equal or longer than the desired length, return it as is
            return input;
        }
        else
        {
            // Calculate the number of zeros to pad
            int zerosToPad = desiredLength - input.Length;

            // Use string.PadLeft to add zeros to the left
            return input.PadLeft(desiredLength, '0');
        }
    }

    public static string RemoveLastNCharacters(string inputString, int n)
    {
        if (n >= inputString.Length)
        {
            // Handle the case where n is greater than or equal to the length of the string
            return string.Empty;
        }

        return inputString.Substring(0, inputString.Length - n);
    }

    public static List<GameObject> FindInactiveObjectsByTag(string tag)
    {
        List<GameObject> outp = new List<GameObject>();
        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].hideFlags == HideFlags.None)
            {
                if (objs[i].tag == tag)
                {
                    outp.Add(objs[i].gameObject);
                }
            }
        }
        return outp;
    }

    public static string RemoveFileExtension(ReadOnlySpan<char> path)
    {
        var lastPeriod = path.LastIndexOf('.');
        return (lastPeriod < 0 ? path : path[..lastPeriod]).ToString();
    }
}
