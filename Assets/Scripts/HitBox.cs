using System;
using UnityEngine;

public enum HitBoxType
{
    Circle,
    Rectangle
}

public class HitBox : MonoBehaviour
{
    public HitBoxType hitBoxType;

    public Vector2 center;
    public float radius = 1f; //Para Circulos

    public Vector2 size = new Vector2(1f, 1f); //Para Rectángulos

    public event Action<HitBox> OnCollisionEnterCustom;

    public void TriggerCollisionEvent(HitBox other)
    {
        OnCollisionEnterCustom?.Invoke(other);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        switch (hitBoxType)
        {
            case HitBoxType.Circle:
                Gizmos.DrawWireSphere(transform.position + (Vector3)center, radius);
                break;
            case HitBoxType.Rectangle:
                Gizmos.DrawWireCube(transform.position + (Vector3)center, size);
                break;
        }
    }

    public Rect GetBounds()
    {
        if(hitBoxType == HitBoxType.Rectangle)
        {
            return new Rect((Vector2)transform.position + center - size / 2, size);
        }

        return new Rect();
    }
}
