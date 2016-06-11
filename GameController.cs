using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Controls the game status, scoring, and also controls the BoardController.
/// </summary>
public class GameController : MonoBehaviour
{
    public static GameController sharedInstance = null;

    private GameObject blackScreen;
    private Text titleText;
    private Button startButton;
    private Text startButtonText;
    private Text messageText;
    private Text scoreText;
    private Text highScoreText;

    private int score = 0;
    private int highScore = 0;
    [HideInInspector]
    public bool canMove = false;
    private bool isReplay = false;

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
        InitializeLevel();
    }

    private void OnLevelWasLoaded(int index)
    {
        InitializeLevel();
        // Immediately deactivate the UI elements and allow movement.
        // At this point the user has already died once,
        // so they don't need to see the start screen again.
        blackScreen.SetActive(false);
        titleText.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);
        canMove = true;
    }

    /// <summary>
    /// Initializes the level and gets references to UI elements.
    /// </summary>
    private void InitializeLevel()
    {
        score = 0;
        BoardController.sharedInstance.CreateWalls();
        BoardController.sharedInstance.CreateSnake();
        BoardController.sharedInstance.CreateCoffee();

        blackScreen = GameObject.Find("BlackScreen");
        titleText = GameObject.Find("TitleText").GetComponent<Text>();
        startButton = GameObject.Find("StartButton").GetComponent<Button>();
        startButtonText = GameObject.Find("StartButtonText").GetComponent<Text>();
        messageText = GameObject.Find("MessageText").GetComponent<Text>();
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        highScoreText = GameObject.Find("HighScoreText").GetComponent<Text>();
        highScoreText.text = "HIGH SCORE: " + highScore;

        startButton.onClick.AddListener(new UnityAction(StartButtonClicked));
    }

    public void IncrementScore()
    {
        score++;
        scoreText.text = "SCORE: " + score;
    }

    public void ReachedMaximumSpeed()
    {
        messageText.text = "MAXIMUM SPEED!";
    }

    /// <summary>
    /// Ends the game, disables the snake, updates the high score,
    /// and displays UI elements allowing the player to restart the game.
    /// </summary>
    public void GameOver()
    {
        canMove = false;
        // Disable the board controller to disable the snake.
        BoardController.sharedInstance.enabled = false;

        titleText.text = "DEAD";
        startButtonText.text = "RESTART";
        if (score > highScore)
        {
            highScore = score;
            highScoreText.text = "HIGH SCORE: " + highScore;
            messageText.text = "NEW HIGH SCORE!";
        }

        // Do not activate blackScreen because the player
        // needs to see the high score and the dead snake.
        titleText.gameObject.SetActive(true);
        startButton.gameObject.SetActive(true);
        isReplay = true;
    }
        
    /// <summary>
    /// Handles the start button OnClick event. Starts or restarts the game.
    /// </summary>
    private void StartButtonClicked()
    {
        if (isReplay)
        {
            // If the player is restarting the game, we need to reload the scene
            // to reset the score and create a new snake.
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            // If the player is starting for the first time, the scene
            // is already loaded. We just need to hide the UI elements.
            blackScreen.SetActive(false);
            titleText.gameObject.SetActive(false);
            startButton.gameObject.SetActive(false);
            canMove = true;
        }
    }
}
