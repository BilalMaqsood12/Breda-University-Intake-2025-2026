using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public static float time;

    [SerializeField] private TextMeshProUGUI timerText;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Level 1")
        {
            time = 0;
        }
    }

    void Update()
    {
        time += Time.deltaTime;

        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        int milliseconds = Mathf.FloorToInt((time * 1000) % 1000);

        timerText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }
}
