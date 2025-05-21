using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{

    [Range(0, 1)][SerializeField] private float coeficiente_e = 1f;

    [SerializeField] private List<HitBox> hitBoxes = new List<HitBox>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hitBoxes = FindObjectsByType<HitBox>(FindObjectsSortMode.None).ToList();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ResolveHitBoxCollisions();
    }

    bool CheckHitBoxCollision(HitBox a, HitBox b)
    {
        if (a.hitBoxType == HitBoxType.Circle && b.hitBoxType == HitBoxType.Circle)
            return CircleCircleCollision(a, b);
        if ((a.hitBoxType == HitBoxType.Circle && b.hitBoxType == HitBoxType.Rectangle))
            return CircleRectCollision(a, b);
        if (a.hitBoxType == HitBoxType.Rectangle && b.hitBoxType == HitBoxType.Circle)
            return CircleRectCollision(b, a);
        if (a.hitBoxType == HitBoxType.Rectangle && b.hitBoxType == HitBoxType.Rectangle)
            return RectRectCollision(a, b);

        return false;
    }

    bool CircleCircleCollision(HitBox a, HitBox b)
    {
        Vector2 centerA = (Vector2)a.transform.position + a.center;
        Vector2 centerB = (Vector2)b.transform.position + b.center;

        float distance = Vector2.Distance(centerA, centerB);
        return distance < (a.radius + b.radius);
    }

    bool CircleRectCollision(HitBox circle, HitBox rect)
    {
        Vector2 circleCenter = (Vector2)circle.transform.position + circle.center;
        Vector2 rectCenter = (Vector2)rect.transform.position + rect.center;
        Vector2 halfSize = rect.size * 0.5f;

        float closestX = Mathf.Clamp(circleCenter.x, rectCenter.x - halfSize.x, rectCenter.x + halfSize.x);
        float closestY = Mathf.Clamp(circleCenter.y, rectCenter.y - halfSize.y, rectCenter.y + halfSize.y);
        Vector2 closestPoint = new Vector2(closestX, closestY);

        return Vector2.Distance(circleCenter, closestPoint) < circle.radius;
    }

    bool RectRectCollision(HitBox a, HitBox b)
    {
        Vector2 centerA = (Vector2)a.transform.position + a.center;
        Vector2 centerB = (Vector2)b.transform.position + b.center;
        Vector2 halfSizeA = a.size * 0.5f;
        Vector2 halfSizeB = b.size * 0.5f;

        return Mathf.Abs(centerA.x - centerB.x) < (halfSizeA.x + halfSizeB.x) &&
               Mathf.Abs(centerA.y - centerB.y) < (halfSizeA.y + halfSizeB.y);
    }


    void ResolveHitBoxCollisions()
    {
        for (int i = 0; i < hitBoxes.Count; i++)
        {
            for (int j = i + 1; j < hitBoxes.Count; j++)
            {
                HitBox a = hitBoxes[i];
                HitBox b = hitBoxes[j];

                if (CheckHitBoxCollision(a, b))
                {
                    // Si ambos tienen movimiento, intercambiar velocidades como colisión elástica
                    var moveA = a.GetComponent<ParticleMovement>();
                    var moveB = b.GetComponent<ParticleMovement>();
                    print($"Colisión entre {a.name} y {b.name}");
                    print($"ParticleMovement A: {moveA}");
                    print($"ParticleMovement B: {moveB}");

                    if (moveA != null && moveB != null)
                    {
                        Vector2 delta = (b.transform.position - a.transform.position).normalized;
                        Vector2 relativeVelocity = moveA.velocity - moveB.velocity;

                        float velocityAlongNormal = Vector2.Dot(relativeVelocity, delta);
                        if (velocityAlongNormal > 0) continue;

                        float e = coeficiente_e;

                        float impulse = (-(1 + e) * velocityAlongNormal) / (moveA.mass + moveB.mass);
                        Vector2 impulseVec = impulse * delta;

                        moveA.velocity -= impulseVec / moveA.mass;
                        moveB.velocity += impulseVec / moveB.mass;
                        print("case 1");
                    }
                    // Si solo uno tiene movimiento, invertir su velocidad
                    else if (moveA != null)
                    {
                        Rect bounds = b.GetBounds(); // Obtener los límites del rectángulo

                        // Encontrar el punto más cercano dentro del rectángulo al centro del círculo
                        float closestX = Mathf.Clamp(a.center.x, bounds.xMin, bounds.xMax);
                        float closestY = Mathf.Clamp(a.center.y, bounds.yMin, bounds.yMax);
                        Vector2 closestPoint = new Vector2(closestX, closestY);

                        // Vector desde el punto más cercano hasta el centro del círculo
                        Vector2 deltaPos = (Vector2)a.transform.position - closestPoint;

                        Vector2 normal = deltaPos.normalized;

                        // Reflejar la velocidad en la normal
                        moveA.velocity = moveA.velocity - 2 * Vector2.Dot(moveA.velocity, normal) * normal;
                        print("case 2");
                    }
                    else if (moveB != null)
                    {
                        Rect bounds = a.GetBounds(); // Obtener los límites del rectángulo

                        // Encontrar el punto más cercano dentro del rectángulo al centro del círculo
                        float closestX = Mathf.Clamp(b.center.x, bounds.xMin, bounds.xMax);
                        float closestY = Mathf.Clamp(b.center.y, bounds.yMin, bounds.yMax);
                        Vector2 closestPoint = new Vector2(closestX, closestY);

                        // Vector desde el punto más cercano hasta el centro del círculo
                        Vector2 deltaPos = (Vector2)b.transform.position - closestPoint;

                        Vector2 normal = deltaPos.normalized;

                        // Reflejar la velocidad en la normal
                        moveB.velocity = moveB.velocity - 2 * Vector2.Dot(moveB.velocity, normal) * normal;
                        print("case 3");
                    }
                }
            }
        }
    }


    /*
    void ResolveCircleCollision()
    {
        for (int i = 0; i < particlesList.Count; i++)
        {
            for (int j = i + 1; j < particlesList.Count; j++)
            {
                CheckAndResolve(particlesList[i], particlesList[j]);
            }
        }
    }
    void ResolveBoundsCollision()
    {
        for (int i = 0; i < particlesList.Count; i++)
        {
            for (int j = 0; j < boundsList.Count; j++)
            {
                CheckAndResolve(particlesList[i], boundsList[j]);
            }
        }
    }

    void CheckAndResolve(ParticleMovement a, BoundsCollider b)
    {
        Rect bounds = b.GetBounds(); // Obtener los límites del rectángulo

        // Encontrar el punto más cercano dentro del rectángulo al centro del círculo
        float closestX = Mathf.Clamp(a.transform.position.x, bounds.xMin, bounds.xMax);
        float closestY = Mathf.Clamp(a.transform.position.y, bounds.yMin, bounds.yMax);
        Vector2 closestPoint = new Vector2(closestX, closestY);

        // Vector desde el punto más cercano hasta el centro del círculo
        Vector2 deltaPos = (Vector2)a.transform.position - closestPoint;
        float distance = deltaPos.magnitude;

        if (distance < a.Radius) // Si hay colisión
        {
            // Normal de colisión
            Vector2 normal = deltaPos.normalized;

            // Reflejar la velocidad en la normal
            a.Velocity = a.Velocity - 2 * Vector2.Dot(a.Velocity, normal) * normal;
            
        }
    }

    void CheckAndResolve(ParticleMovement a, ParticleMovement b)
    {
        Vector2 deltaPos = b.transform.position - a.transform.position;
        float distance = deltaPos.magnitude;
        float minDistance = a.Radius + b.Radius;

        if (distance < minDistance)
        {

            Vector2 normal = deltaPos.normalized;
            Vector2 tangente = new Vector2(-normal.y, normal.x);

            float v1n = Vector2.Dot(a.velocity, normal);
            float v2n = Vector2.Dot(b.velocity, normal);
            float v1t = Vector2.Dot(a.velocity, tangente);
            float v2t = Vector2.Dot(b.velocity, tangente);

            float vA_nFinal = ((a.mass - coeficiente_e * b.mass) * v1n + (1 + coeficiente_e) * b.mass * v2n) / (a.mass + b.mass);
            float vB_nFinal = ((b.mass - coeficiente_e * a.mass) * v2n + (1 + coeficiente_e) * b.mass * v1n) / (a.mass + b.mass);

            Vector2 vA_nVector = vA_nFinal * normal;
            Vector2 vA_tVector = v1t * tangente;
            Vector2 vB_nVector = vB_nFinal * normal;
            Vector2 vB_tVector = v2t * tangente;

            a.velocity = vA_nVector + vA_tVector;
            b.velocity = vB_nVector + vB_tVector;
        }
    }
    */

}

