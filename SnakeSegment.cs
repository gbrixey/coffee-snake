using UnityEngine;
using System.Collections;
using Direction = MovementController.Direction;

public class SnakeSegment : MonoBehaviour
{
    public SnakeSegment nextSegment;
    public SnakeSegment previousSegment;
    public Direction previousSegmentDirection;

    protected SpriteRenderer spriteRenderer;
    protected Rigidbody2D rigidBody;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    public virtual void UpdateSprite(Direction nextSegmentDirection)
    {
        if (nextSegmentDirection == Direction.up)
        {
            if (previousSegmentDirection == Direction.left)
            {
                spriteRenderer.sprite = SpriteController.sharedInstance.lowerRightCornerSprite;
            }
            else if (previousSegmentDirection == Direction.right)
            {
                spriteRenderer.sprite = SpriteController.sharedInstance.lowerLeftCornerSprite;
            }
            else if (previousSegmentDirection == Direction.down)
            {
                spriteRenderer.sprite = SpriteController.sharedInstance.verticalSprite;
            }
        }
        else if (nextSegmentDirection == Direction.left)
        {
            if (previousSegmentDirection == Direction.up)
            {
                spriteRenderer.sprite = SpriteController.sharedInstance.lowerRightCornerSprite;
            }
            else if (previousSegmentDirection == Direction.right)
            {
                spriteRenderer.sprite = SpriteController.sharedInstance.horizontalSprite;
            }
            else if (previousSegmentDirection == Direction.down)
            {
                spriteRenderer.sprite = SpriteController.sharedInstance.upperRightCornerSprite;
            }
        }
        else if (nextSegmentDirection == Direction.right)
        {
            if (previousSegmentDirection == Direction.up)
            {
                spriteRenderer.sprite = SpriteController.sharedInstance.lowerLeftCornerSprite;
            }
            else if (previousSegmentDirection == Direction.left)
            {
                spriteRenderer.sprite = SpriteController.sharedInstance.horizontalSprite;
            }
            else if (previousSegmentDirection == Direction.down)
            {
                spriteRenderer.sprite = SpriteController.sharedInstance.upperLeftCornerSprite;
            }
        }
        else if (nextSegmentDirection == Direction.down)
        {
            if (previousSegmentDirection == Direction.up)
            {
                spriteRenderer.sprite = SpriteController.sharedInstance.verticalSprite;
            }
            else if (previousSegmentDirection == Direction.left)
            {
                spriteRenderer.sprite = SpriteController.sharedInstance.upperRightCornerSprite;
            }
            else if (previousSegmentDirection == Direction.right)
            {
                spriteRenderer.sprite = SpriteController.sharedInstance.upperLeftCornerSprite;
            }
        }
    }

    protected virtual void MoveTo(Vector2 newPosition)
    {
        Vector2 oldPosition = transform.position;
        Direction directionToMove = GetDirection(oldPosition, newPosition);
        rigidBody.MovePosition(newPosition);
        UpdateSprite(MovementController.OppositeDirection(directionToMove));

        nextSegment.previousSegmentDirection = directionToMove;
        nextSegment.MoveTo(oldPosition);
    }

    protected Direction GetDirection(Vector2 startingPosition, Vector2 endingPosition)
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
