using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PopupViewController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Button previousView, nextView;
    [SerializeField] List<GameObject> viewContentList = new();

    private int currentView = 0;

    // Start is called before the first frame update
    void Start()
    {
        Initialise();
    }

    void OnDestroy()
    {

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
        currentView++;
        ActivateView(currentView);
    }

    private void GoToPreviousView()
    {
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

        if (currentView != viewContentList.Count - 1)
        {
            nextView.gameObject.SetActive(true);
        }
    }

    private void ActivateView(int currentView)
    {
        foreach (var view in viewContentList)
        {
            view.gameObject.SetActive(false);
        }

        viewContentList[currentView].gameObject.SetActive(true);

        UpdateButtonsUI();
    }

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
