using UnityEngine;

public class FruitsLevelHandler : MonoBehaviour
{
    public GameObject[] fruitsPrefab;
    public int maxNumOfFruits = 5;
    public int numOfFruitsLeft;

    public bool isGameOver = false;
    public Canvas gameOverCanvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        numOfFruitsLeft = maxNumOfFruits;
    }

    private void Update()
    {
        if(numOfFruitsLeft <= 0)
        {
            isGameOver = true;
            // Poner Game Over Canvas
            Debug.Log("All fruits have been used.");
        }
    }
    public GameObject GetRandomFruitPrefab()
    {
        if (fruitsPrefab.Length == 0) return null;
        return fruitsPrefab[Random.Range(0, fruitsPrefab.Length)];
    }

}
