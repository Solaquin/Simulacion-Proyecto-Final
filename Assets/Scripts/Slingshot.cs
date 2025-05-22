using UnityEngine;

public class Slingshot : MonoBehaviour
{
    public GameObject[] fruitPrefab;
    public float fuerzaMultiplicadora = 10f;
    public float maxDragDistance = 3f;

    public TrajectoryPreview preview;
    public FollowTarget cameraFollow;

    private Vector2 dragStart, dragEnd;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            dragEnd = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 rawForce = dragStart - dragEnd;
            Vector2 clamped = Vector2.ClampMagnitude(rawForce, maxDragDistance);
            Vector2 velocity = clamped * fuerzaMultiplicadora;

            preview.ShowTrajectory(transform.position, velocity);
        }

        if (Input.GetMouseButtonUp(0))
        {
            dragEnd = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 rawForce = dragStart - dragEnd;
            Vector2 clamped = Vector2.ClampMagnitude(rawForce, maxDragDistance);
            Vector2 velocity = clamped * fuerzaMultiplicadora;

            LaunchFruit(velocity);
            preview.HideTrajectory();
        }
    }

    void LaunchFruit(Vector2 velocity)
    {
        GameObject bird = Instantiate(fruitPrefab[0], transform.position, Quaternion.identity);
        cameraFollow.target = bird.transform;
        cameraFollow.offset = new Vector2(0, 0);
        ParticleMovement movement = bird.GetComponent<ParticleMovement>();
        movement.velocity = velocity;
    }
}
