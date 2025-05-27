using UnityEngine;

public class LevelCompleted : MonoBehaviour
{
    public bool isLevelCompleted = false;
    public Canvas levelPassedCanvas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //levelPassedCanvas.gameObject.SetActive(false);
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
                //levelPassedCanvas.gameObject.SetActive(true);
            }
        }
    }
}
