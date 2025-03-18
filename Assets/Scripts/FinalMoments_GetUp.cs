using System.Collections;
using UnityEngine;

public class FinalMoments_GetUp : MonoBehaviour
{
    public GameObject sleepyBody;
    public GameObject awakeBody;
    public CanvasGroup whiteScreen;
    public GameObject awokenBodyDialogues;
    public Timer timer;
    public GameObject endScreen;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Time.timeScale = 0f;
            LeanTween.alphaCanvas(whiteScreen, 1, 1f).setIgnoreTimeScale(true).setOnComplete(() =>
            {
                GameManager.instance.player.gameObject.SetActive(false);
                timer.gameObject.SetActive(false);
                sleepyBody.SetActive(false);
                awakeBody.SetActive(true);
                
                LeanTween.alphaCanvas(whiteScreen, 0, 1f).setDelay(1f).setIgnoreTimeScale(true).setOnComplete(() =>
                {
                    Time.timeScale = 1f;
                    StartCoroutine(TransitionOnEndScreen());
                });
            });
        }
    }

    private IEnumerator TransitionOnEndScreen()
    {
        yield return new WaitForSeconds(1f);
        awokenBodyDialogues.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        awokenBodyDialogues.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        endScreen.gameObject.SetActive(true);
        CanvasGroup canvasGroup = endScreen.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        LeanTween.alphaCanvas(canvasGroup, 1f, 2f);
    }
}
