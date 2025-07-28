using UnityEngine;
using UnityEngine.UI;

public class PopupBackground : MonoBehaviour
{
    [SerializeField] Button btn;
    void OnEnable()
    {
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(()=>transform.parent.gameObject.SetActive(false));
    }
}
