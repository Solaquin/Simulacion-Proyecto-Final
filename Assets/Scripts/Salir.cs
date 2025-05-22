using UnityEngine;

public class Salir : MonoBehaviour
{
    public void Exit()
    {
        // Esto funciona solo en la versión compilada del juego
        Application.Quit();

        // Esto es útil para pruebas dentro del editor
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}