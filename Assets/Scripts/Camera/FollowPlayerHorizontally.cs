using UnityEngine;

public class FollowPlayerHorizontally : MonoBehaviour
{
    public float minX;
    public float maxX;

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = new Vector2(GameManager.instance.player.position.x, transform.position.y);
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        transform.position = pos;
    }
}
