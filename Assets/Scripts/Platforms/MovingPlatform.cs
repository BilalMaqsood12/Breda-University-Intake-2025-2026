using UnityEngine;

public enum Destination { PointA, PointB }
public class MovingPlatform : MonoBehaviour
{
    public Destination defaultDestination;
    Destination currentDestination;

    public Transform platform;

    public Transform PointATransform;
    public Transform PointBTransform;

    [Space]

    public float speed;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentDestination = defaultDestination;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentDestination == Destination.PointA)
        {
            platform.position = Vector2.MoveTowards(platform.position, PointATransform.position, Time.deltaTime * speed);
            if (Vector2.Distance(platform.position, PointATransform.position) < 0.05f)
                currentDestination = Destination.PointB;
        }

        if (currentDestination == Destination.PointB)
        {
            platform.position = Vector2.MoveTowards(platform.position, PointBTransform.position, Time.deltaTime * speed);
            if (Vector2.Distance(platform.position, PointBTransform.position) < 0.05f)
                currentDestination = Destination.PointA;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (PointATransform != null)
            Gizmos.DrawWireCube(PointATransform.position, new Vector2(0.25f, 0.25f));

        Gizmos.color = Color.white;
        if (PointBTransform != null)
            Gizmos.DrawWireCube(PointBTransform.position, new Vector2(0.25f, 0.25f));

    }

}
