using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSwitcher : MonoBehaviour
{
    public string levelToSwitchWith = "Level Name";
    public GameObject levelCompleteUI;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Time.timeScale > 1)
                Time.timeScale = 1f;

            levelCompleteUI.SetActive(true);
            GameManager.instance.player.GetComponent<PlayerController>().canMove = false;
        }
    }

    public void Continue()
    {
        SceneManager.LoadScene(levelToSwitchWith);

    }

}
