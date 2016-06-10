using UnityEngine;
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

        BoardController.sharedInstance.CreateWalls();
        BoardController.sharedInstance.InitializeSnake();
        BoardController.sharedInstance.PlaceCoffee();
    }

    public void GameOver()
    {
        enabled = false;
        BoardController.sharedInstance.enabled = false;
    }
}
