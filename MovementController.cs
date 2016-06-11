using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour
{
    public enum Direction
    {
        up,
        left,
        right,
        down
    };

    public Direction currentDirection;
    private Direction nextDirection;

    void Awake()
    {
        Time.timeScale = 0.08f;
        currentDirection = Direction.right;
        nextDirection = Direction.right;
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        if (x > float.Epsilon && AllowedDirection(Direction.right))
        {
            nextDirection = Direction.right;
        }
        else if (x < -float.Epsilon && AllowedDirection(Direction.left))
        {
            nextDirection = Direction.left;
        }
        else if (y > float.Epsilon && AllowedDirection(Direction.up))
        {
            nextDirection = Direction.up;
        }
        else if (y < -float.Epsilon && AllowedDirection(Direction.down))
        {
            nextDirection = Direction.down;
        }
    }

    public void UpdateDirection()
    {
        currentDirection = nextDirection;
    }

    public void IncreaseMovementSpeed()
    {
        if (Time.timeScale < 0.16)
        {
            Time.timeScale += 0.002f;
        }
    }

    private bool AllowedDirection(Direction direction)
    {
        switch (currentDirection)
        {
            case Direction.left:
            case Direction.right:
                return (direction == Direction.up || direction == Direction.down);
            case Direction.up:
            case Direction.down:
                return (direction == Direction.left || direction == Direction.right);
            default:
                return false;
        }
    }

    public static Direction GetDirection(Vector2 startingPosition, Vector2 endingPosition)
    {
        Vector2 relativeVector = endingPosition - startingPosition;
        if (relativeVector.x > float.Epsilon)
        {
            return Direction.right;
        }
        else if (relativeVector.x < -float.Epsilon)
        {
            return Direction.left;
        }
        else if (relativeVector.y > float.Epsilon)
        {
            return Direction.up;
        }
        else
        {
            return Direction.down;
        }
    }
}
