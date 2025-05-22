using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TrajectoryPreview : MonoBehaviour
{
    public int numPoints = 30;
    public float timeStep = 0.02f; // Igual a FixedDeltaTime
    public float g = 9.81f;
    public float c = 1.5f;
    public float mass = 1f;

    private LineRenderer line;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
    }

    public void ShowTrajectory(Vector2 startPos, Vector2 initialVelocity)
    {
        line.positionCount = numPoints;

        Vector2 position = startPos;
        Vector2 velocity = initialVelocity;

        for (int i = 0; i < numPoints; i++)
        {
            velocity.x += (-(c / mass) * velocity.x) * timeStep;
            velocity.y += (-g - (c / mass) * velocity.y) * timeStep;

            position += velocity * timeStep;

            line.SetPosition(i, new Vector3(position.x, position.y, 0f));
        }

        line.enabled = true;
    }

    public void HideTrajectory()
    {
        line.enabled = false;
    }
}
