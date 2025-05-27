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
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        hitBoxes = FindObjectsByType<HitBox>(FindObjectsSortMode.None).ToList();
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
        return distance <= (a.radius + b.radius);
    }

    bool CircleRectCollision(HitBox circle, HitBox rect)
    {
        Vector2 circleCenter = (Vector2)circle.transform.position + circle.center;
        Vector2 rectCenter = (Vector2)rect.transform.position + rect.center;
        Vector2 halfSize = rect.size * 0.5f;

        float closestX = Mathf.Clamp(circleCenter.x, rectCenter.x - halfSize.x, rectCenter.x + halfSize.x);
        float closestY = Mathf.Clamp(circleCenter.y, rectCenter.y - halfSize.y, rectCenter.y + halfSize.y);
        Vector2 closestPoint = new Vector2(closestX, closestY);

        return Vector2.Distance(circleCenter, closestPoint) <= circle.radius;
    }

    bool RectRectCollision(HitBox a, HitBox b)
    {
        Vector2 centerA = (Vector2)a.transform.position + a.center;
        Vector2 centerB = (Vector2)b.transform.position + b.center;
        Vector2 halfSizeA = a.size * 0.5f;
        Vector2 halfSizeB = b.size * 0.5f;

        return Mathf.Abs(centerA.x - centerB.x) <= (halfSizeA.x + halfSizeB.x) &&
               Mathf.Abs(centerA.y - centerB.y) <= (halfSizeA.y + halfSizeB.y);
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
                    //Obtener componentes ParticleMovement
                    ParticleMovement moveAComponent = a.GetComponent<ParticleMovement>();
                    ParticleMovement moveBComponent = b.GetComponent<ParticleMovement>();

                    //Verificar si el componente no es nulo o es estático
                    ParticleMovement moveA = (moveAComponent != null && !moveAComponent.isStatic) ? moveAComponent : null;
                    ParticleMovement moveB = (moveBComponent != null && !moveBComponent.isStatic) ? moveBComponent : null;

                    if (a.isTrigger || b.isTrigger) //Si alguno es un trigger, no resolver física, activar eventos
                    {
                        a.TriggerCollisionEvent(b);
                        b.TriggerCollisionEvent(a);
                        continue;
                    }

                    if (moveA == null && moveB == null) continue; // Si ninguno tiene movimiento, no hacer nada


                    if ((moveA != null && moveB != null))
                    {
                        Vector2 deltaPos = b.transform.position - a.transform.position;
                        float distance = deltaPos.magnitude;

                        Vector2 normal = deltaPos.normalized;
                        Vector2 tangente = new Vector2(-normal.y, normal.x);

                        float v1n = Vector2.Dot(moveA.velocity, normal);
                        float v2n = Vector2.Dot(moveB.velocity, normal);
                        float v1t = Vector2.Dot(moveA.velocity, tangente);
                        float v2t = Vector2.Dot(moveB.velocity, tangente);

                        float vA_nFinal = ((moveA.mass - coeficiente_e * moveB.mass) * v1n + (1 + coeficiente_e) * moveB.mass * v2n) / (moveA.mass + moveB.mass);
                        float vB_nFinal = ((moveB.mass - coeficiente_e * moveA.mass) * v2n + (1 + coeficiente_e) * moveB.mass * v1n) / (moveA.mass + moveB.mass);

                        Vector2 vA_nVector = vA_nFinal * normal;
                        Vector2 vA_tVector = v1t * tangente;
                        Vector2 vB_nVector = vB_nFinal * normal;
                        Vector2 vB_tVector = v2t * tangente;

                        moveA.velocity = vA_nVector + vA_tVector;
                        moveB.velocity = vB_nVector + vB_tVector;

                        float penetration = GetPenetrationDepth(a, b, (Vector2)a.transform.position + a.center, (Vector2)b.transform.position + b.center);
                        Vector2 correction = (penetration / 2f) * normal;
                        a.transform.position -= (Vector3)correction;
                        b.transform.position += (Vector3)correction;


                        print("case 1");
                    }
                    // Si solo uno tiene movimiento, invertir su velocidad
                    else if (moveA != null)
                    {
                        Rect bounds = b.GetBounds(); // Obtener los límites del rectángulo

                        // Encontrar el punto más cercano dentro del rectángulo al centro del círculo
                        float closestX = Mathf.Clamp(a.transform.position.x + a.center.x, bounds.xMin, bounds.xMax);
                        float closestY = Mathf.Clamp(a.transform.position.y + a.center.y, bounds.yMin, bounds.yMax);
                        Vector2 closestPoint = new Vector2(closestX, closestY);

                        // Vector desde el punto más cercano hasta el centro del círculo
                        Vector2 deltaPos = (Vector2)a.transform.position + a.center - closestPoint;

                        Vector2 normal = deltaPos.normalized;

                        // Reflejar la velocidad en la normal
                        moveA.velocity = moveA.velocity - 2 * Vector2.Dot(moveA.velocity, normal) * normal;
                        float penetration = GetPenetrationDepth(a, b, (Vector2)a.transform.position + a.center, (Vector2)b.transform.position + b.center);
                        Vector2 correction = penetration * normal;
                        a.transform.position += (Vector3)correction;
                        print("case 2");
                    }
                    else if (moveB != null)
                    {
                        Rect bounds = a.GetBounds(); // Obtener los límites del rectángulo

                        // Encontrar el punto más cercano dentro del rectángulo al centro del círculo
                        float closestX = Mathf.Clamp(b.transform.position.x + b.center.x, bounds.xMin, bounds.xMax);
                        float closestY = Mathf.Clamp(b.transform.position.y + b.center.y, bounds.yMin, bounds.yMax);
                        Vector2 closestPoint = new Vector2(closestX, closestY);

                        // Vector desde el punto más cercano hasta el centro del círculo
                        Vector2 deltaPos = (Vector2)b.transform.position + b.center - closestPoint;

                        Vector2 normal = deltaPos.normalized;

                        // Reflejar la velocidad en la normal
                        moveB.velocity = moveB.velocity - 2 * Vector2.Dot(moveB.velocity, normal) * normal;

                        float penetration = GetPenetrationDepth(b, a, (Vector2)b.transform.position + b.center, (Vector2)a.transform.position + a.center);
                        Vector2 correction = penetration * normal;
                        b.transform.position += (Vector3)correction;
                        print("case 3");
                    }

                    // Llamar al evento de colisión
                    a.TriggerCollisionEvent(b);
                    b.TriggerCollisionEvent(a);
                }
            }
        }

    }
    float GetPenetrationDepth(HitBox a, HitBox b, Vector2 centerA, Vector2 centerB)
    {
        if (a.hitBoxType == HitBoxType.Circle && b.hitBoxType == HitBoxType.Circle)
        {
            return (a.radius + b.radius) - Vector2.Distance(centerA, centerB);
        }

        if (a.hitBoxType == HitBoxType.Circle && b.hitBoxType == HitBoxType.Rectangle)
        {
            Vector2 halfSize = b.size * 0.5f;
            Vector2 closest = new Vector2(
                Mathf.Clamp(centerA.x, centerB.x - halfSize.x, centerB.x + halfSize.x),
                Mathf.Clamp(centerA.y, centerB.y - halfSize.y, centerB.y + halfSize.y)
            );
            return a.radius - Vector2.Distance(centerA, closest);
        }

        if (a.hitBoxType == HitBoxType.Rectangle && b.hitBoxType == HitBoxType.Circle)
        {
            return GetPenetrationDepth(b, a, centerB, centerA);
        }

        // Aproximación para rectángulo contra rectángulo (solo en eje menor)
        if (a.hitBoxType == HitBoxType.Rectangle && b.hitBoxType == HitBoxType.Rectangle)
        {
            Vector2 halfA = a.size * 0.5f;
            Vector2 halfB = b.size * 0.5f;

            float dx = (halfA.x + halfB.x) - Mathf.Abs(centerA.x - centerB.x);
            float dy = (halfA.y + halfB.y) - Mathf.Abs(centerA.y - centerB.y);

            return Mathf.Min(dx, dy);
        }

        return 0f;
    }

}

