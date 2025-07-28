using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewController : MonoBehaviour
{
    [SerializeField] PopupController popupController;
    [SerializeField] Button previousView, nextView;
    [SerializeField] GameObject advanceOptions;
    [SerializeField] List<GameObject> viewContentList = new();
    [SerializeField] AudioClip changeViewClip;

    private int currentView = 0;

    // Start is called before the first frame update
    void Start()
    {
        Initialise();
        popupController.PopupOpening += Initialise;
    }

    void OnDestroy()
    {
        popupController.PopupOpening -= Initialise;
    }

    void Initialise()
    {
        previousView.onClick.RemoveAllListeners();
        nextView.onClick.RemoveAllListeners();

        previousView.onClick.AddListener(GoToPreviousView);
        nextView.onClick.AddListener(GoToNextView);

        // set view 1 
        currentView = 0;
        ActivateView(currentView);
        // update buttons UI (if no available view before or after disable button)
        UpdateButtonsUI();
    }

    private void GoToNextView()
    {
        AudioManager.instance.PlaySound(changeViewClip);
        currentView++;
        ActivateView(currentView);
    }

    private void GoToPreviousView()
    {
        AudioManager.instance.PlaySound(changeViewClip);
        currentView--;
        ActivateView(currentView);
    }

    private void UpdateButtonsUI()
    {
        nextView.gameObject.SetActive(false);
        previousView.gameObject.SetActive(false);

        if (currentView != 0)
        {
            previousView.gameObject.SetActive(true);
        }

        if (currentView != viewContentList.Count-1)
        {
            nextView.gameObject.SetActive(true);
        }

        if (currentView == viewContentList.Count - 1)
        {
            advanceOptions.SetActive(true);
        }
        else
        {
            advanceOptions.SetActive(false);
        }
    }

    private void ActivateView(int currentView)
    {
        foreach (var view in  viewContentList)
        {
            view.gameObject.SetActive(false);
        }

        viewContentList[currentView].gameObject.SetActive(true);

        UpdateButtonsUI();
    }
}
