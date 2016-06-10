using UnityEngine;
using System.Collections;
using Direction = MovementController.Direction;

public class SnakeTailSegment : SnakeSegment
{
    public override void UpdateSprite(Direction nextSegmentDirection)
    {
        UpdateSprite();
    }

    protected override void MoveTo(Vector2 newPosition)
    {
        Vector2 oldPosition = transform.position;
        rigidBody.MovePosition(newPosition);
        BoardController.sharedInstance.SnakeLeftPosition(oldPosition);
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        switch (previousSegmentDirection)
        {
            case Direction.up:
                spriteRenderer.sprite = SpriteController.sharedInstance.downTailSprite;
                break;
            case Direction.left:
                spriteRenderer.sprite = SpriteController.sharedInstance.rightTailSprite;
                break;
            case Direction.right:
                spriteRenderer.sprite = SpriteController.sharedInstance.leftTailSprite;
                break;
            case Direction.down:
                spriteRenderer.sprite = SpriteController.sharedInstance.upTailSprite;
                break;
        }
    }
}
