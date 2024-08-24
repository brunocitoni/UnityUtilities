using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ButtonIgnoreTransparency : MonoBehaviour
{
    void OnEnable()
    {
        if (!GetComponent<Image>().mainTexture.isReadable)
        {
            Debug.LogError("You have no set the texture for button " + name + " as read/write enabled! You can do so in the Texture 2D Inspector window");
            return;
        }

        GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
    }
}
