using UnityEngine;
using System.Collections;

public class SpriteController : MonoBehaviour
{
    public Sprite horizontalSprite;
    public Sprite verticalSprite;
    public Sprite upperLeftCornerSprite;
    public Sprite upperRightCornerSprite;
    public Sprite lowerRightCornerSprite;
    public Sprite lowerLeftCornerSprite;
    public Sprite leftTailSprite;
    public Sprite rightTailSprite;
    public Sprite upTailSprite;
    public Sprite downTailSprite;
    public Sprite leftHeadSprite;
    public Sprite rightHeadSprite;
    public Sprite upHeadSprite;
    public Sprite downHeadSprite;
    public Sprite leftBiteSprite;
    public Sprite rightBiteSprite;
    public Sprite upBiteSprite;
    public Sprite downBiteSprite;
    public Sprite leftDeadSprite;
    public Sprite rightDeadSprite;
    public Sprite upDeadSprite;
    public Sprite downDeadSprite;

    public static SpriteController sharedInstance;

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
    }
}
