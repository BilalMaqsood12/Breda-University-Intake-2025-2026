using Unity.Cinemachine;
using UnityEngine;

public class SwitchCamera : MonoBehaviour
{
    public CinemachineCamera from;
    public CinemachineCamera to;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!to.enabled)
            {
                from.enabled = false;
                to.enabled = true;
            
                CameraManager.instance.currentCamera = to;
            }
            else
            {
                from.enabled = true;
                to.enabled = false;
                
                CameraManager.instance.currentCamera = from;
            }

        }
    }
}
