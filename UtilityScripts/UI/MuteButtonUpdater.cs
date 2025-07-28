using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MuteButtonUpdater : MonoBehaviour
{
    public Sprite mutedAudioSprite;
    public Sprite activeAudioSprite;

    private Button thisButton;
    private Image buttonImage;

    private void Start()
    {
        thisButton = GetComponent<Button>();
        buttonImage = thisButton.GetComponentInChildren<Image>();

        thisButton.onClick.RemoveAllListeners();
        thisButton.onClick.AddListener(OnClickToggleMute);

        UpdateUI();
    }

    public void OnClickToggleMute()
    {
        AudioManager.instance.ToggleMute();
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (AudioManager.instance.isMuted)
        {
            buttonImage.sprite = mutedAudioSprite;
        }
        else
        {
            buttonImage.sprite = activeAudioSprite;
        }
    }

}
