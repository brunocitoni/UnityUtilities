/*
 * ButtonLongPressHandler.cs
 * 
 * This script handles long press and click events for a button in Unity UI.
 * It allows you to define a hold time for triggering a long press event and a maximum duration for triggering a click event after pressing the button.
 * 
 * Usage:
 * 1. Attach this script to a UI button object.
 * 2. Set the holdTime and clickDuration parameters in the inspector.
 * 3. Add functions to the onClick and onLongPress UnityEvents to define the behavior when the button is clicked or long pressed.
 * 
 * This script implements the IPointerDownHandler, IPointerUpHandler, and IPointerExitHandler interfaces to handle pointer events on the button.
 * 
 * Author: Bruno Citoni
 */

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
///     Handles long press and click events for a button in Unity UI.
/// </summary>
public class ButtonLongPressHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [SerializeField]
    [Tooltip("How long must pointer be down on this object to trigger a long press")]
    private float holdTime = 1.5f;

    [SerializeField]
    [Tooltip("Maximum duration to trigger the onClick event after pressing the button")]
    private float clickDuration = 0.3f;

    public UnityEvent onClick = new();
    public UnityEvent onLongPress = new();
    
    private bool isLongPressTriggered;
    private bool isPointerDown;
    private float pointerDownTimer;

    private void Update()
    {
        if (isPointerDown)
        {
            pointerDownTimer += Time.deltaTime;

            if (pointerDownTimer >= holdTime && !isLongPressTriggered)
            {
                isLongPressTriggered = true;
                onLongPress.Invoke();
            }
        }
    }

    /// <summary>
    ///     Called when the pointer is pressed down on the button.
    /// </summary>
    /// <param name="eventData">The pointer event data.</param>
    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true;
        pointerDownTimer = 0f;
    }

    /// <summary>
    ///     Called when the pointer exits the button.
    /// </summary>
    /// <param name="eventData">The pointer event data.</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerDown = false;
        pointerDownTimer = 0f;
    }

    /// <summary>
    ///     Called when the pointer is released from the button.
    /// </summary>
    /// <param name="eventData">The pointer event data.</param>
    public void OnPointerUp(PointerEventData eventData)
    {
        if (isLongPressTriggered)
        {
            isPointerDown = false;
            isLongPressTriggered = false;
        }
        else
        {
            if (pointerDownTimer <= clickDuration) onClick.Invoke();

            isPointerDown = false;
        }
    }
}