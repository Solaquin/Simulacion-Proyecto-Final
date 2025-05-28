using System.Collections;
using UnityEngine;

public class LevelCompleted : MonoBehaviour
{
    public bool isLevelCompleted = false;
    public Canvas levelPassedCanvas;

    public AudioClip levelCompletedSound;
    private AudioSource audioSource;
    private bool clipSoundPlayed = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        levelPassedCanvas.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        GameObject[] currentEnemies = GameObject.FindGameObjectsWithTag("Enemys");

        if (currentEnemies.Length == 0)
        {
            if (!isLevelCompleted)
            {
                isLevelCompleted = true;
                Debug.Log("Level Completed!");
                StartCoroutine(activeCanvasBeforeDelay(3f));
            }
        }
    }

    public IEnumerator activeCanvasBeforeDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (!clipSoundPlayed)
        {
            audioSource.PlayOneShot(levelCompletedSound);
            clipSoundPlayed = true;
        }
        levelPassedCanvas.gameObject.SetActive(true);
    }
}
