using UnityEngine;

public class Slingshot : MonoBehaviour
{
    public GameObject[] fruitPrefab;
    public float fuerzaMultiplicadora = 10f;
    public float maxDragDistance = 3f;

    public TrajectoryPreview preview;
    public FollowTarget cameraFollow;
    public GameObject point;

    private Animator animator;
    private Vector2 dragStart, dragEnd;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            animator.SetBool("isLaunching", true);
        }

        if (Input.GetMouseButton(0))
        {
            dragEnd = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 rawForce = dragStart - dragEnd;
            Vector2 clamped = Vector2.ClampMagnitude(rawForce, maxDragDistance);
            Vector2 velocity = clamped * fuerzaMultiplicadora;

            preview.ShowTrajectory(point.transform.position, velocity);
        }

        if (Input.GetMouseButtonUp(0))
        {
            dragEnd = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 rawForce = dragStart - dragEnd;
            Vector2 clamped = Vector2.ClampMagnitude(rawForce, maxDragDistance);
            Vector2 velocity = clamped * fuerzaMultiplicadora;
            animator.SetBool("isLaunching", false);
            LaunchFruit(velocity);
            preview.HideTrajectory();
        }
    }

    void LaunchFruit(Vector2 velocity)
    {
        GameObject fruit = Instantiate(fruitPrefab[0], point.transform.position, Quaternion.identity);
        cameraFollow.target = fruit.transform;
        cameraFollow.offset = new Vector2(0, 0);
        ParticleMovement movement = fruit.GetComponent<ParticleMovement>();
        movement.velocity = velocity;
    }
}
