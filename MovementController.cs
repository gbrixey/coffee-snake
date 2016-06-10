using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour
{
    public enum Direction {left, right, up, down};

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

    public static Direction OppositeDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.left:
                return Direction.right;
            case Direction.right:
                return Direction.left;
            case Direction.up:
                return Direction.down;
            case Direction.down:
            default:
                return Direction.up;
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
}
