using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{
    private static MusicPlayer instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Configura un valor predeterminado si no existe
        if (!PlayerPrefs.HasKey("volumeAudio"))
        {
            PlayerPrefs.SetFloat("volumeAudio", 1.0f); // Valor por defecto (100% volumen)
            PlayerPrefs.Save();
        }

        // Aplica el valor de PlayerPrefs al volumen global
        AudioListener.volume = PlayerPrefs.GetFloat("volumeAudio");
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Nombres de las escenas iniciales
        string[] initialScenes = { "Menu_Principal", "Ajustes", "Niveles" };

        if (System.Array.Exists(initialScenes, sceneName => sceneName == scene.name))
        {
            if (!GetComponent<AudioSource>().isPlaying)
                GetComponent<AudioSource>().Play();
        }
        else
        {
            GetComponent<AudioSource>().Stop();
        }
    }
}