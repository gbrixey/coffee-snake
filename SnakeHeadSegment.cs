using UnityEngine;
using System.Collections;
using Direction = MovementController.Direction;

public class SnakeHeadSegment : SnakeSegment
{
    public MovementController movementController;
    private BoxCollider2D boxCollider;

    private float movementAmount = 1.0f;
    private bool addSegmentOnNextMove = false;

    protected override void Awake()
    {
        base.Awake();
        movementController = GetComponent<MovementController>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void FixedUpdate()
    {
        if (enabled)
        {
            movementController.UpdateDirection();
            AttemptMoveTo(NextPosition());
        }
    }

    private void OnDisable()
    {
        Direction nextSegmentDirection = MovementController.GetDirection(nextSegment.transform.position, transform.position);
        switch (nextSegmentDirection)
        {
            case Direction.up:
                spriteRenderer.sprite = SpriteController.sharedInstance.upDeadSprite;
                break;
            case Direction.left:
                spriteRenderer.sprite = SpriteController.sharedInstance.leftDeadSprite;
                break;
            case Direction.right:
                spriteRenderer.sprite = SpriteController.sharedInstance.rightDeadSprite;
                break;
            case Direction.down:
                spriteRenderer.sprite = SpriteController.sharedInstance.downDeadSprite;
                break;
        }
    }

    public override void UpdateSprite()
    {
        boxCollider.enabled = false;
        RaycastHit2D hit = Physics2D.Linecast(transform.position, NextPosition());
        boxCollider.enabled = true;

        bool aboutToGetCoffee = (hit.transform != null && hit.transform.GetComponent<Coffee>() != null);
        spriteRenderer.sprite = SpriteController.sharedInstance.GetSnakeHeadSprite(movementController.currentDirection, aboutToGetCoffee);
    }

    protected override void MoveTo(Vector2 newPosition)
    {
        if (addSegmentOnNextMove)
        {
            addSegmentOnNextMove = false;

            Vector2 oldPosition = transform.position;
            transform.position = newPosition;

            SnakeSegment newSegment = BoardController.sharedInstance.NewSnakeSegment(oldPosition);
            newSegment.nextSegment = nextSegment;
            newSegment.nextSegment.previousSegment = newSegment;
            newSegment.previousSegment = this;
            newSegment.UpdateSprite();

            nextSegment = newSegment;
            UpdateSprite();
        }
        else
        {
            base.MoveTo(newPosition);
        }
        BoardController.sharedInstance.SnakeEnteredPosition(newPosition);
    }

    private void AttemptMoveTo(Vector2 newPosition)
    {
        boxCollider.enabled = false;
        RaycastHit2D hit = Physics2D.Linecast(transform.position, newPosition);
        boxCollider.enabled = true;

        if (hit.transform != null)
        {
            if (hit.transform.GetComponent<SnakeSegment>() != null ||
                hit.transform.GetComponent<Wall>() != null)
            {
                GameController.sharedInstance.GameOver();
            }
            else
            {
                Coffee coffee = hit.transform.GetComponent<Coffee>();
                if (coffee != null)
                {
                    MoveTo(newPosition);
                    BoardController.sharedInstance.MoveCoffee(coffee.gameObject);
                    movementController.IncreaseMovementSpeed();
                    addSegmentOnNextMove = true;
                }
                else
                {
                    MoveTo(newPosition);
                }
            }
        }
        else
        {
            MoveTo(newPosition);
        }
    }

    private Vector2 NextPosition()
    {
        Vector2 newPosition = transform.position;
        switch (movementController.currentDirection)
        {
            case Direction.up:
                newPosition.y += movementAmount;
                break;
            case Direction.left:
                newPosition.x -= movementAmount;
                break;
            case Direction.right:
                newPosition.x += movementAmount;
                break;
            case Direction.down:
                newPosition.y -= movementAmount;
                break;
        }
        return newPosition;
    }
}
