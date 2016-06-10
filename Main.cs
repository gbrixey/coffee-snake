using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour
{
    public GameObject gameController;
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
