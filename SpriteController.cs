using UnityEngine;
using System.Collections;
using Direction = MovementController.Direction;

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

        DontDestroyOnLoad(gameObject);
    }

    public Sprite GetSnakeHeadSprite(Direction movementDirection, bool bite, bool dead)
    {
        switch (movementDirection)
        {
            case Direction.up:
                return dead ? upDeadSprite : (bite ? upBiteSprite : upHeadSprite);
            case Direction.left:
                return dead ? leftDeadSprite : (bite ? leftBiteSprite : leftHeadSprite);
            case Direction.right:
                return dead ? rightDeadSprite : (bite ? rightBiteSprite : rightHeadSprite);
            case Direction.down:
                return dead ? downDeadSprite : (bite ? downBiteSprite : downHeadSprite);
            default:
                return null;
        }
    }

    public Sprite GetSnakeTailSprite(Direction previousSegmentDirection)
    {
        switch (previousSegmentDirection)
        {
            case Direction.up:
                return downTailSprite;
            case Direction.left:
                return rightTailSprite;
            case Direction.right:
                return leftTailSprite;
            case Direction.down:
                return upTailSprite;
            default:
                return null;
        }
    }

    public Sprite GetSnakeSegmentSprite(Direction previousSegmentDirection, Direction nextSegmentDirection)
    {
        switch (nextSegmentDirection)
        {
            case Direction.up:
                switch (previousSegmentDirection)
                {
                    case Direction.left:
                        return lowerRightCornerSprite;
                    case Direction.right:
                        return lowerLeftCornerSprite;
                    case Direction.down:
                        return verticalSprite;
                    default:
                        return null;
                }
            case Direction.left:
                switch (previousSegmentDirection)
                {
                    case Direction.up:
                        return lowerRightCornerSprite;
                    case Direction.right:
                        return horizontalSprite;
                    case Direction.down:
                        return upperRightCornerSprite;
                    default:
                        return null;
                }
            case Direction.right:
                switch (previousSegmentDirection)
                {
                    case Direction.up:
                        return lowerLeftCornerSprite;
                    case Direction.left:
                        return horizontalSprite;
                    case Direction.down:
                        return upperLeftCornerSprite;
                    default:
                        return null;
                }
            case Direction.down:
                switch (previousSegmentDirection)
                {
                    case Direction.up:
                        return verticalSprite;
                    case Direction.left:
                        return upperRightCornerSprite;
                    case Direction.right:
                        return upperLeftCornerSprite;
                    default:
                        return null;
                }
            default:
                return null;
        }
    }
}
