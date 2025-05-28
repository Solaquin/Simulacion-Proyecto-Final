using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FruitsLevelHandler : MonoBehaviour
{
    public GameObject[] fruitsPrefab;
    public int maxNumOfFruits = 5;
    public int numOfFruitsLeft;

    public bool isGameOver = false;
    public Canvas gameOverCanvas;
    public Image fruitImage;

    public AudioClip gameOverSound;
    private AudioSource audioSource;
    bool clipSoundPlayed = false;

    private LevelCompleted levelCompleted;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        levelCompleted = GetComponent<LevelCompleted>();
        gameOverCanvas.gameObject.SetActive(false);
        numOfFruitsLeft = maxNumOfFruits;
    }

    private void Update()
    {
        if (numOfFruitsLeft <= 0)
        {
            if(!levelCompleted.isLevelCompleted)
            {
                isGameOver = true;
                StartCoroutine(activeCanvasBeforeDelay(3f));
                Debug.Log("All fruits have been used.");
            }
        }
    }
    public GameObject GetRandomFruitPrefab()
    {
        if (fruitsPrefab.Length == 0) return null;
        return fruitsPrefab[Random.Range(0, fruitsPrefab.Length)];
    }

    public void SetFruitImage(Sprite fruitSprite)
    {
        if (fruitImage != null)
        {
            fruitImage.sprite = fruitSprite;
        }
    }

    public IEnumerator activeCanvasBeforeDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (!clipSoundPlayed)
        {
            audioSource.PlayOneShot(gameOverSound);
            clipSoundPlayed = true;
        }
        gameOverCanvas.gameObject.SetActive(true);
    }

}
