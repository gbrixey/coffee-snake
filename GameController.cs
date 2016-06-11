using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

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
        Initialize();
    }

    private void OnLevelWasLoaded(int index)
    {
        Initialize();
        blackScreen.SetActive(false);
        titleText.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);
        canMove = true;
    }

    private void Initialize()
    {
        score = 0;
        BoardController.sharedInstance.CreateWalls();
        BoardController.sharedInstance.InitializeSnake();
        BoardController.sharedInstance.PlaceCoffee();

        blackScreen = GameObject.Find("BlackScreen");
        titleText = GameObject.Find("TitleText").GetComponent<Text>();
        startButton = GameObject.Find("StartButton").GetComponent<Button>();
        startButtonText = GameObject.Find("StartButtonText").GetComponent<Text>();
        messageText = GameObject.Find("MessageText").GetComponent<Text>();
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        highScoreText = GameObject.Find("HighScoreText").GetComponent<Text>();
        highScoreText.text = "HIGH SCORE: " + highScore;

        startButton.onClick.AddListener(new UnityAction(Restart));
    }

    public void IncrementScore()
    {
        score++;
        scoreText.text = "SCORE: " + score;
        if (MovementController.IsMaximumSpeed())
        {
            messageText.text = "MAXIMUM SPEED!";
        }
    }

    public void GameOver()
    {
        canMove = false;
        titleText.text = "DEAD";
        startButtonText.text = "RESTART";
        if (score > highScore)
        {
            highScore = score;
            highScoreText.text = "HIGH SCORE: " + highScore;
            messageText.text = "NEW HIGH SCORE!";
        }

        titleText.gameObject.SetActive(true);
        startButton.gameObject.SetActive(true);
        BoardController.sharedInstance.enabled = false;
        isReplay = true;
    }

    private void Restart()
    {
        if (isReplay)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            blackScreen.SetActive(false);
            titleText.gameObject.SetActive(false);
            startButton.gameObject.SetActive(false);
            canMove = true;
        }
    }
}
