using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; 

public class SceneTransition : MonoBehaviour
{
    public Animator transitionAnimator;
    public float transitionTime = 1f; // duración del fade

    public void LoadSceneWithFade(string sceneName)
    {
        StartCoroutine(Transition(sceneName));
    }

    IEnumerator Transition(string sceneName)
    {
        // Ejecuta el fade out
        transitionAnimator.SetTrigger("StartFadeOut");

        // Espera a que termine la animación
        yield return new WaitForSeconds(transitionTime);

        // Cambia de escena
        SceneManager.LoadScene(sceneName);
    }
}
