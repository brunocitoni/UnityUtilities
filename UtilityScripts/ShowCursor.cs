using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShowCursor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Implement IPointerEnterHandler interface method
    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.visible = true;
    }

    // Implement IPointerExitHandler interface method
    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.visible = false;
    }
}
