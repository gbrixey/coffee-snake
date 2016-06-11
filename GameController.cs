using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameController : MonoBehaviour
{
    public static GameController sharedInstance = null;

    void Awake()
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
        }
        else if (sharedInstance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        BoardController.sharedInstance.CreateWalls();
        BoardController.sharedInstance.InitializeSnake();
        BoardController.sharedInstance.PlaceCoffee();
    }

    public void GameOver()
    {
        enabled = false;
        BoardController.sharedInstance.enabled = false;
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
