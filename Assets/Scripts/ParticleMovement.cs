using UnityEngine;
using static Unity.Burst.Intrinsics.X86;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ParticleMovement : MonoBehaviour
{
    public float mass = 1f;
    public Vector2 velocity = Vector2.zero;

    [SerializeField] private float g = 9.81f, c = 1.5f;
    public bool isStatic = false;

    private Vector2 position;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        position = transform.position;
    }
    void FixedUpdate()
    {
        if (isStatic) return;

        position = transform.position;
            
        // Aplica gravedad y fricción lineal
        velocity.x += (-(c / mass) * velocity.x) * Time.fixedDeltaTime;
        velocity.y += (-g - (c / mass) * velocity.y) * Time.fixedDeltaTime;

        // Actualiza posición
        position += velocity * Time.fixedDeltaTime;
        transform.position = position;
    }
}
