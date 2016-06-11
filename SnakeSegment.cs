using UnityEngine;
using System.Collections;
using Direction = MovementController.Direction;

public class SnakeSegment : MonoBehaviour
{
    public SnakeSegment nextSegment;
    public SnakeSegment previousSegment;

    protected SpriteRenderer spriteRenderer;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public virtual void UpdateSprite()
    {
        Direction previousSegmentDirection = MovementController.GetDirection(transform.position, previousSegment.transform.position);
        Direction nextSegmentDirection = MovementController.GetDirection(transform.position, nextSegment.transform.position);

        spriteRenderer.sprite = SpriteController.sharedInstance.GetSnakeSegmentSprite(previousSegmentDirection, nextSegmentDirection);
    }

    protected virtual void MoveTo(Vector2 newPosition)
    {
        Vector2 oldPosition = transform.position;
        transform.position = newPosition;
        nextSegment.MoveTo(oldPosition);
        UpdateSprite();
    }
}
