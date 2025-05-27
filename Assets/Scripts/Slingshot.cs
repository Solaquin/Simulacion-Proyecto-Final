using System.Collections;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    public FruitsLevelHandler levelHandler;
    public LevelCompleted levelCompletedHandler;
    public float fuerzaMultiplicadora = 10f;
    public float maxDragDistance = 3f;

    public TrajectoryPreview preview;
    public GameObject point;
    public FollowTarget followTarget;    

    private Animator animator;
    private Vector2 dragStart, dragEnd;
    private GameObject currentFruitPrefab;
    private bool canLaunch = true;


    private void Start()
    {
        animator = GetComponent<Animator>();

        currentFruitPrefab = levelHandler.GetRandomFruitPrefab(); // inicial
        levelHandler.SetFruitImage(currentFruitPrefab.GetComponent<SpriteRenderer>().sprite);
    }

    void Update()
    {
        if (levelHandler.isGameOver || levelCompletedHandler.isLevelCompleted) return;
        if (!canLaunch) return;

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
            levelHandler.numOfFruitsLeft--;
            canLaunch = false;

        }
    }

    void LaunchFruit(Vector2 velocity)
    {
        GameObject fruit = Instantiate(currentFruitPrefab, point.transform.position, Quaternion.identity);
        followTarget.currentTarget = fruit.transform;
        followTarget.currentOffset = new Vector2(0, 0);
        ParticleMovement movement = fruit.GetComponent<ParticleMovement>();
        movement.velocity = velocity;

        currentFruitPrefab = levelHandler.GetRandomFruitPrefab();
        levelHandler.SetFruitImage(currentFruitPrefab.GetComponent<SpriteRenderer>().sprite);
    }
    public void NotifyFruitDestroyed()
    {
        if (!canLaunch)
        {
            canLaunch = true;
        }
    }

}
