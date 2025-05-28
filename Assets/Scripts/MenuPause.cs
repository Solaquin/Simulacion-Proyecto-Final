using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuPause : MonoBehaviour
{

    private bool pausado = false;
    [SerializeField] private GameObject canva;
    [SerializeField] private SceneTransition sceneManager;

    private LevelCompleted LevelCompleted;
    private FruitsLevelHandler gameOver;
    [SerializeField] private Slingshot slingshot;

    void Start()
    {
        canva.SetActive(false);
        LevelCompleted = GetComponent<LevelCompleted>();
        gameOver = GetComponent<FruitsLevelHandler>();
    }


    public void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape) && !LevelCompleted.isLevelCompleted && !gameOver.isGameOver)
        {
            AlternarPausa();
        }

    }

    public void AlternarPausa()
    {

        pausado = !pausado;

        if (pausado)
        {
            slingshot.enabled = false;
            canva.SetActive(true);
            Time.timeScale = 0f;

        }
        else
        {
            slingshot.enabled = true;
            canva.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void Continuar()
    {
        slingshot.enabled = true;
        Debug.Log("Continuar");
        canva.SetActive(false);
        Time.timeScale = 1f;
    }


    public void Reiniciar()
    {
        Debug.Log("Reiniciar");
        Time.timeScale = 1f;
        sceneManager.LoadSceneWithFade(SceneManager.GetActiveScene().name);
        
    }


    public void Salir()
    {
        canva.SetActive(false);
        Time.timeScale = 1f;
        sceneManager.LoadSceneWithFade("Menu_Principal");
    }
}
