using System.Collections;
using UnityEngine;

public class FruitsBehaviour : MonoBehaviour
{
    private HitBox hitBox;
    private FollowTarget followTarget;
    private Slingshot slingshot;

    private AudioSource audioSource;
    public AudioClip audioScream;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        followTarget = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FollowTarget>();
        slingshot = GameObject.FindGameObjectWithTag("Slingshot").GetComponent<Slingshot>();
        hitBox = GetComponent<HitBox>();
    }

    void OnEnable()
    {
        hitBox.OnCollisionEnterCustom += OnCustomCollision;
        audioSource.PlayOneShot(audioScream);
    }

    void OnDisable()
    {
        hitBox.OnCollisionEnterCustom -= OnCustomCollision;
        
    }

    private void OnDestroy()
    {
        slingshot.NotifyFruitDestroyed(); // Reset the slingshot state when the fruit is destroyed
        followTarget.resetTarget();
    }

    void OnCustomCollision(HitBox other)
    {       
        StartCoroutine(destroyAfterDelay(5f));       
    }

    public IEnumerator destroyAfterDelay(float delay)
    {

        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
