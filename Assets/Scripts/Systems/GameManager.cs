using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Transform player;
    public Transform startingPoint;

    [Header("GAMEPLAY")]
    public bool enableSpeedBooster;
    public float maxSpeedToBoost = 1.25f;
    public float speedBoosterDuration = 6f;

    [HideInInspector] public Checkpoint currentCheckpoint;


    private void Awake()
    {
        instance = this;

    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TeleportPlayerTo(startingPoint);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void TeleportPlayerTo(Transform point)
    {
        player.position = point.position;
    }


}
