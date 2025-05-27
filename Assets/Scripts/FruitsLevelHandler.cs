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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameOverCanvas.gameObject.SetActive(false);
        numOfFruitsLeft = maxNumOfFruits;
    }

    private void Update()
    {
        if(numOfFruitsLeft <= 0)
        {
            isGameOver = true;
            StartCoroutine(activeCanvasBeforeDelay(1.5f));
            Debug.Log("All fruits have been used.");
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
        gameOverCanvas.gameObject.SetActive(true);
    }

}
