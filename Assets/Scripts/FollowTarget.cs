using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 2f;
    public Vector2 offset;

    void Update()
    {
        if (target == null) return;
        Vector2 desired = (Vector2)target.position + offset;
        transform.position = Vector2.Lerp(transform.position, desired, Time.deltaTime * smoothSpeed);
    }
}