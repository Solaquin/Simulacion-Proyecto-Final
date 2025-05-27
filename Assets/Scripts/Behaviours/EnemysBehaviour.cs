using System.Collections;
using UnityEngine;

public class EnemysBehaviour : MonoBehaviour
{
    private int numHits = 0;
    public int maxHits = 2;

    private HitBox hitBox;

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
            numHits++;
            print(other.name + " hit " + gameObject.name + ", total hits: " + numHits);
            if (numHits < maxHits)
            {
                //Añadir sonidos o instanciar particulas
                return;
            }
            else
            {
                StartCoroutine(destroyAfterDelay(0.5f));
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
