using UnityEngine;
using System.Collections;
using Direction = MovementController.Direction;

/// <summary>
/// Provides sprites to the snake segments based on
/// their position relative to other snake segments.
/// </summary>
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

    /// <summary>
    /// The three different types of snake head sprite.
    /// </summary>
    public enum SnakeHeadSpriteType
    {
        normal,
        biting,
        dead
    }

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

    /// <summary>
    /// Gets the snake head sprite of the given type.
    /// </summary>
    public Sprite GetSnakeHeadSprite(Direction movementDirection, SnakeHeadSpriteType type)
    {
        switch (movementDirection)
        {
            case Direction.up:
                switch (type)
                {
                    case SnakeHeadSpriteType.biting:
                        return upBiteSprite;
                    case SnakeHeadSpriteType.dead:
                        return upDeadSprite;
                    case SnakeHeadSpriteType.normal:
                    default:
                        return upHeadSprite;
                }
            case Direction.left:
                switch (type)
                {
                    case SnakeHeadSpriteType.biting:
                        return leftBiteSprite;
                    case SnakeHeadSpriteType.dead:
                        return leftDeadSprite;
                    case SnakeHeadSpriteType.normal:
                    default:
                        return leftHeadSprite;
                }
            case Direction.right:
                switch (type)
                {
                    case SnakeHeadSpriteType.biting:
                        return rightBiteSprite;
                    case SnakeHeadSpriteType.dead:
                        return rightDeadSprite;
                    case SnakeHeadSpriteType.normal:
                    default:
                        return rightHeadSprite;
                }
            case Direction.down:
                switch (type)
                {
                    case SnakeHeadSpriteType.biting:
                        return downBiteSprite;
                    case SnakeHeadSpriteType.dead:
                        return downDeadSprite;
                    case SnakeHeadSpriteType.normal:
                    default:
                        return downHeadSprite;
                }
            default:
                return null;
        }
    }

    /// <summary>
    /// Gets the snake tail sprite based on the
    /// direction of the previous snake segment.
    /// </summary>
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

    /// <summary>
    /// Gets an appropriate snake body sprite for the given positions
    /// of the previous and next snake segments.
    /// </summary>
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
