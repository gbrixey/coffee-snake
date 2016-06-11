using UnityEngine;
using System.Collections;

/// <summary>
/// Translates player input into a relative direction
/// that determines where the snake will move next.
/// </summary>
public class MovementController : MonoBehaviour
{
    /// <summary>
    /// Relative directions: up, left, right, and down.
    /// </summary>
    public enum Direction
    {
        up,
        left,
        right,
        down
    };

    private const float initialTimeScale = 0.08f;
    private const float timeScaleIncrement = 0.002f;
    private const float maximumTimeScale = 0.18f;

    /// <summary>
    /// The direction in which the snake is currently moving.
    /// </summary>
    public Direction currentDirection;

    /// <summary>
    /// The next direction the snake should move,
    /// according to player input and movement restrictions.
    /// </summary>
    private Direction nextDirection;

    private void Awake()
    {
        Time.timeScale = initialTimeScale;
        currentDirection = Direction.right;
        nextDirection = Direction.right;
    }

    private void Update()
    {
        // Determine which direction the player wants to move and if this
        // is an allowed direction, and if so, update nextDirection accordingly.
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

    /// <summary>
    /// Updates the snake's current movement direction to nextDirection.
    /// </summary>
    public void UpdateDirection()
    {
        currentDirection = nextDirection;
    }

    public void IncreaseMovementSpeed()
    {
        if (Time.timeScale < maximumTimeScale)
        {
            Time.timeScale += timeScaleIncrement;
            if (Time.timeScale >= maximumTimeScale)
            {
                GameController.sharedInstance.ReachedMaximumSpeed();
            }
        }
    }

    /// <summary>
    /// Whether or not the snake is allowed to move in the given direction.
    /// </summary>
    private bool AllowedDirection(Direction direction)
    {
        // Do not allow the snake to move in the exact opposite direction
        // from the current direction, since this would mean the snake would
        // immediately overlap itself and die.

        // Also return false for the current direction in order to give
        // priority to other directions in the Update function.
        // i.e. if the snake is moving right and the user is holding down the
        // right and down keys, we want to move the snake down.
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

    /// <summary>
    /// Gets the relative direction from one position to another.
    /// Assumes the positions only differ in one dimension (x or y).
    /// </summary>
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
