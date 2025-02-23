using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    public Transform[] horizontalFollowTransforms;
    public Transform[] verticalFollowTransforms;


    CinemachineCamera[] allCinemachineCamsInScene;
    CinemachineCamera defaultCam;

    private void Awake()
    {
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        allCinemachineCamsInScene = GameObject.FindObjectsByType<CinemachineCamera>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (CinemachineCamera cam in allCinemachineCamsInScene)
        {
            if (cam.IsLive)
                defaultCam = cam;
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var followTransform in horizontalFollowTransforms)
        {
            followTransform.position = new Vector2(GameManager.instance.player.position.x, followTransform.position.y);
        }

        foreach (var followTransform in verticalFollowTransforms)
        {
            followTransform.position = new Vector2(followTransform.position.x, GameManager.instance.player.position.y);
        }
    }

    public void ResetToDefaultCamera()
    {
        foreach (CinemachineCamera cam in allCinemachineCamsInScene)
        {
            cam.enabled = false;
        }

        defaultCam.enabled = true;
    }
}
