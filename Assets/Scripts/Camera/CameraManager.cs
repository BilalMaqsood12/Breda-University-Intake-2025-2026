using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    public CinemachineCamera defaultCam;

    public Transform[] horizontalFollowTransforms;
    public Transform[] verticalFollowTransforms;


    CinemachineCamera[] allCinemachineCamsInScene;

    [HideInInspector] public CinemachineCamera currentCamera;

    private void Awake()
    {
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        allCinemachineCamsInScene = GameObject.FindObjectsByType<CinemachineCamera>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        currentCamera = defaultCam;

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

    public void SwitchCameraTo(CinemachineCamera cameraToEnable)
    {
        foreach (CinemachineCamera cam in allCinemachineCamsInScene)
        {
            if (cam != cameraToEnable)
            {
                cam.enabled = false;
            }
        }
        cameraToEnable.enabled = true;

        currentCamera = cameraToEnable;
    }

    public void ResetToDefaultCamera()
    {
        foreach (CinemachineCamera cam in allCinemachineCamsInScene)
        {
            cam.enabled = false;
        }

        defaultCam.enabled = true;
        currentCamera = defaultCam;
    }


    public void CameraShake(Vector3 value)
    {
        StartCoroutine(CameraShakeCouroutine(value.x, value.y, value.z));
    }

    private IEnumerator CameraShakeCouroutine(float amplitude, float frequecy, float time)
    {
        CinemachineBasicMultiChannelPerlin cm_Noise = currentCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
        cm_Noise.AmplitudeGain = amplitude;
        cm_Noise.FrequencyGain = frequecy;
        yield return new WaitForSeconds(time);
        cm_Noise.AmplitudeGain = 0;
        cm_Noise.FrequencyGain = 0;
    }

}
