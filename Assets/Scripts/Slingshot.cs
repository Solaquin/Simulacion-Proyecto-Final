using UnityEngine;

public class Slingshot : MonoBehaviour
{
    public GameObject birdPrefab;
    private Vector2 dragStart, dragEnd;
    private GameObject currentBird;
    public float fuerzaMultiplicadora = 10f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            dragStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonUp(0))
        {
            dragEnd = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 force = (dragStart - dragEnd) * fuerzaMultiplicadora;
            LaunchBird(force);
        }
    }

    void LaunchBird(Vector2 velocity)
    {
        currentBird = Instantiate(birdPrefab, transform.position, Quaternion.identity);
        var movement = currentBird.GetComponent<ParticleMovement>();
        movement.velocity = velocity;
    }
}