using UnityEngine;
using System.Collections;
using Direction = MovementController.Direction;

public class SnakeTailSegment : SnakeSegment
{
    public override void UpdateSprite()
    {
        Direction previousSegmentDirection = MovementController.GetDirection(transform.position, previousSegment.transform.position);
        spriteRenderer.sprite = SpriteController.sharedInstance.GetSnakeTailSprite(previousSegmentDirection);
    }

    protected override void MoveTo(Vector2 newPosition)
    {
        Vector2 oldPosition = transform.position;
        transform.position = newPosition;
        BoardController.sharedInstance.SnakeLeftPosition(oldPosition);
        UpdateSprite();
    }
}
