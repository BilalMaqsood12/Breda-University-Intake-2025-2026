using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Checkpoint : MonoBehaviour
{
    public Color checkpointColor;
    public SpriteRenderer background;
    public CinemachineCamera nearbyCamera;
    public Transform spawnPos;
    public GameObject checkpointText;

    [HideInInspector] public float savedTimeScale;

    bool saved;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !saved)
        {
            LeanTween.color(background.gameObject, checkpointColor, 0.25f);
            GameManager.instance.currentCheckpoint = this;
            Debug.Log("Checkpoint!");

            savedTimeScale = Time.timeScale;
            saved = true;

            checkpointText.SetActive(false); //Disable checkpoint text in case it's enabled.
            checkpointText.SetActive(true);
        }
    }
}
