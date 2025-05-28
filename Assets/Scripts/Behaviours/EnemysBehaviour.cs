using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class EnemysBehaviour : MonoBehaviour
{
    private int numHits = 0;
    public int maxHits = 2;

    private HitBox hitBox;
    private AudioSource audioSource;
    public AudioClip hitSound;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
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
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(hitSound);
            }
            numHits++;
            print(other.name + " hit " + gameObject.name + ", total hits: " + numHits);
            if (numHits < maxHits)
            {
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
