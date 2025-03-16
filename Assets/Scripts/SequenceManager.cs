using UnityEngine;

public class SequenceManager : MonoBehaviour
{
    public GameObject playableDirector;
    public GameObject[] objectsToEnableDisable;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playableDirector.SetActive(true);
        foreach (var item in objectsToEnableDisable)
        {
            item.SetActive(false);
        }
    }

    public void EndSequence()
    {
        playableDirector.SetActive(false);
        foreach (var item in objectsToEnableDisable)
        {
            item.SetActive(true);
        }
    }

}
