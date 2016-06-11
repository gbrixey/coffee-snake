using UnityEngine;
using System.Collections;

/// <summary>
/// Starts the game by instantiating the
/// GameController and other singletons.
/// </summary>
public class Main : MonoBehaviour
{
    public GameController gameController;
    public SpriteController spriteController;
    public BoardController boardController;

    void Awake()
    {
        if (SpriteController.sharedInstance == null)
        {
            Instantiate(spriteController);
        }
        if (BoardController.sharedInstance == null)
        {
            Instantiate(boardController);
        }
        if (GameController.sharedInstance == null)
        {
            Instantiate(gameController);
        }
	}
}
