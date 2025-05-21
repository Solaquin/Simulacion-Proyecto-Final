using UnityEngine;

public class BoundsCollider : MonoBehaviour
{
    private Vector2 size;

    private void Start()
    {
        size = transform.lossyScale; // Tamaño en coordenadas globales
    }

    // Devuelve los límites del rectángulo en coordenadas globales
    public Rect GetBounds()
    {
        Vector2 center = (Vector2)transform.position;
        Vector2 min = center - size / 2;
        return new Rect(min, size);
    }

    // Dibuja un gizmo en el editor para visualizar el rectángulo
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, transform.lossyScale);
    }
}
