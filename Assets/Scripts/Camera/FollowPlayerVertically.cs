using UnityEngine;

public class FollowPlayerVertically : MonoBehaviour
{
    public float minY;
    public float maxY;

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = new Vector2(transform.position.x, GameManager.instance.player.position.y);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        transform.position = pos;
    }
}
