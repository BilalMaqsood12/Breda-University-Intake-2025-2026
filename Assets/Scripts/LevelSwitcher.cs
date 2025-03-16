using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSwitcher : MonoBehaviour
{
    public string levelToSwitchWith = "Level Name";
    public GameObject blackScreen;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Time.timeScale > 1)
                Time.timeScale = 1f;

            blackScreen.SetActive(true);
            StartCoroutine(SwitchScenes());
            GameManager.instance.player.GetComponent<PlayerController>().canMove = false;
        }
    }

    IEnumerator SwitchScenes()
    {
        yield return new WaitForSecondsRealtime(1);
        SceneManager.LoadScene(levelToSwitchWith);

    }

}
