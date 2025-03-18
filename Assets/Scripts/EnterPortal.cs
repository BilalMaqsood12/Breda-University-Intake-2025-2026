using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterPortal : MonoBehaviour
{
    public GameObject whiteScreen;
    public string transitionToScene = "Final Moments";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Time.timeScale = 0f;
            LeanTween.alphaCanvas(whiteScreen.GetComponent<CanvasGroup>(), 1f, 1f).setDelay(1f).setIgnoreTimeScale(true).setOnComplete(()=>
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene(transitionToScene);
            });
        }
    }
}
