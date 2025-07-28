using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{ 
    [SerializeField] Fader sceneFader;
    [SerializeField][Range(0.0f, 1f)] float fadeOutTime;
    [SerializeField][Range(0.0f, 1f)] float fadeInTime;

    private void Awake()
    {
        sceneFader.SetVisible(true);
        StartCoroutine(sceneFader.FadeOut(fadeOutTime));
    }
    public void FadeToScene(string scene)
    {
        StartCoroutine(FadeOutAndLoadScene(scene));
    }

    private IEnumerator FadeOutAndLoadScene(string sceneName)
    {
        yield return StartCoroutine(sceneFader.FadeIn(fadeInTime));
        SceneManager.LoadSceneAsync(sceneName);
    }
}
