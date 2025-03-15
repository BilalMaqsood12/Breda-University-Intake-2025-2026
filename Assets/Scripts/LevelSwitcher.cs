using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSwitcher : MonoBehaviour
{
    public string levelToSwitchWith = "Level Name";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Time.timeScale > 1)
                Time.timeScale = 1f;

            SceneManager.LoadScene(levelToSwitchWith);
        }
    }

}
