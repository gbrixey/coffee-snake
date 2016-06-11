using UnityEngine;
using System.Collections;
using Direction = MovementController.Direction;

public class SnakeTailSegment : SnakeSegment
{
    /// <summary>
    /// Updates the tail sprite based on the position of the previous segment.
    /// </summary>
    public override void UpdateSprite()
    {
        Direction previousSegmentDirection = MovementController.GetDirection(transform.position, previousSegment.transform.position);
        spriteRenderer.sprite = SpriteController.sharedInstance.GetSnakeTailSprite(previousSegmentDirection);
    }

    /// <summary>
    /// Moves the snake tail to the given position and informs
    /// the game controller that the snake has left the old position.
    /// </summary>
    protected override void MoveTo(Vector2 newPosition)
    {
        Vector2 oldPosition = transform.position;
        transform.position = newPosition;
        BoardController.sharedInstance.SnakeLeftPosition(oldPosition);
        UpdateSprite();
    }
}
