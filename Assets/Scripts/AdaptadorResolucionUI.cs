using UnityEngine;
using UnityEngine.UI;

// Este atributo hace que el script también se ejecute en el editor (modo edición)
[ExecuteAlways]

// Asegura que el componente RectTransform exista en este GameObject
[RequireComponent(typeof(RectTransform))]
public class AdaptadorResolucionUI : MonoBehaviour
{
    // Resolución base de referencia (por defecto: Full HD)
    public Vector2 resolucionReferencia = new Vector2(1920f, 1080f);

    // Referencias internas al componente RectTransform
    private RectTransform rectTransform;

    // Guardamos la posición y el tamaño original (en la resolución base)
    private Vector2 posicionOriginal;
    private Vector2 tamanoOriginal;

    void Awake()
    {
        // Obtenemos el componente RectTransform del objeto actual
        rectTransform = GetComponent<RectTransform>();

        // Guardamos la posición y tamaño iniciales como referencia
        posicionOriginal = rectTransform.anchoredPosition;
        tamanoOriginal = rectTransform.sizeDelta;
    }

    void Update()
    {
        if (rectTransform == null) return;

        // Obtenemos la resolución actual de la pantalla
        float anchoActual = Screen.width;
        float altoActual = Screen.height;

        // Calculamos los factores de escala respecto a la resolución base
        float escalaX = anchoActual / resolucionReferencia.x;
        float escalaY = altoActual / resolucionReferencia.y;

        // Adaptamos la posición original con la escala actual
        rectTransform.anchoredPosition = new Vector2(
            posicionOriginal.x * escalaX,
            posicionOriginal.y * escalaY
        );

        // Adaptamos el tamaño original con la escala actual
        rectTransform.sizeDelta = new Vector2(
            tamanoOriginal.x * escalaX,
            tamanoOriginal.y * escalaY
        );
    }
}
