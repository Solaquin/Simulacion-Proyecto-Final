using System.Collections;
using UnityEngine;

public class StructuresBehaviour : MonoBehaviour
{
    private int numHits = 0;
    public int maxHits = 2;

    private HitBox hitBox;
    private Animator animator;

    void Awake()
    {
        hitBox = GetComponent<HitBox>();
        animator = GetComponent<Animator>();
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
            numHits++;
            if (numHits < maxHits)
            {
                animator.SetBool("Collide", true);
            }
            else
            {
                animator.SetBool("Collide", true);
                StartCoroutine(destroyAfterDelay(1f));
            }   
        }
    }

    public IEnumerator destroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        destroyStructure();
    }

    public void destroyStructure()
    {
        if (numHits >= maxHits)
        {
            Destroy(gameObject);
        }
    }
}
