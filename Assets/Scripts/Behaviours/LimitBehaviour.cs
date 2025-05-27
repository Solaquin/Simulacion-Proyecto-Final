using System.Collections;
using UnityEngine;

public class LimitBehaviour : MonoBehaviour
{
    private HitBox hitBox;
    public FollowTarget followTarget;


    void Awake()
    {
        hitBox = GetComponent<HitBox>();
    }

    void OnEnable()
    {
        hitBox.OnCollisionEnterCustom += OnCustomCollision;
    }

    void OnDisable()
    {
        hitBox.OnCollisionEnterCustom -= OnCustomCollision;
    }

    void OnCustomCollision(HitBox other)
    {
        if (other.CompareTag("Fruits"))
        {
            Destroy(other.gameObject);
        }
    }
}
