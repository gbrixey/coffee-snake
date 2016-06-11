using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

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
    private Button quitButton;
    private Text messageText;
    private Text scoreText;
    private Text highScoreText;

    private static string highScorePath = Application.persistentDataPath + "/highscore.gd";

    private int score = 0;
    private int highScore = 0;
    [HideInInspector]
    public bool canMove = false;
    private bool isReplay = false;

    private void Awake()
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
        LoadHighScore();
        InitializeLevel();
    }

    private void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
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
        quitButton.gameObject.SetActive(false);
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
        quitButton = GameObject.Find("QuitButton").GetComponent<Button>();
        messageText = GameObject.Find("MessageText").GetComponent<Text>();
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        highScoreText = GameObject.Find("HighScoreText").GetComponent<Text>();
        highScoreText.text = "HIGH SCORE: " + highScore;

        startButton.onClick.AddListener(new UnityAction(StartButtonClicked));
        quitButton.onClick.AddListener(new UnityAction(QuitButtonClicked));
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
            SaveHighScore();
        }

        // Do not activate blackScreen because the player
        // needs to see the high score and the dead snake.
        titleText.gameObject.SetActive(true);
        startButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
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
            quitButton.gameObject.SetActive(false);
            canMove = true;
        }
    }

    /// <summary>
    /// Handles the quit button OnClick event. Quits the application.
    /// </summary>
    private void QuitButtonClicked()
    {
        Application.Quit();
    }

    /// <summary>
    /// Tries to load an existing high score from the game data file.
    /// </summary>
    private void LoadHighScore()
    {
        if (File.Exists(highScorePath))
        {
            FileStream file = File.OpenRead(highScorePath);
            BinaryFormatter bf = new BinaryFormatter();
            highScore = (int)bf.Deserialize(file);
            file.Close();
        }
    }

    /// <summary>
    /// Saves the high score to a game data file.
    /// </summary>
    private void SaveHighScore()
    {
        FileStream file = File.Create(highScorePath);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, highScore);
        file.Close();
    }
}
