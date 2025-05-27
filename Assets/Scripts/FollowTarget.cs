using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform normalTarget;
    public Transform currentTarget;
    public float smoothSpeed = 2f;
    public Vector2 offset;
    public Vector2 currentOffset;

    private void Start()
    {
        currentTarget = normalTarget;
        currentOffset = offset;
    }

    void Update()
    {
        if (currentTarget == null) return;
        Vector2 desired = (Vector2)currentTarget.position + currentOffset;
        transform.position = Vector2.Lerp(transform.position, desired, Time.deltaTime * smoothSpeed);
    }

    public void resetTarget()
    {
        currentTarget = normalTarget;
        currentOffset = offset;
    }
}