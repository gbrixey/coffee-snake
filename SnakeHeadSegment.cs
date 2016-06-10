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
            AttemptMoveTo(PositionAfterMoving(1));
        }
    }

    private void OnDisable()
    {
        Direction nextSegmentDirection = GetDirection(nextSegment.transform.position, transform.position);
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

    public override void UpdateSprite(Direction nextSegmentDirection)
    {
        boxCollider.enabled = false;
        RaycastHit2D hit = Physics2D.Linecast(transform.position, PositionAfterMoving(2));
        boxCollider.enabled = true;

        bool aboutToGetCoffee = (hit.transform != null && hit.transform.GetComponent<Coffee>() != null);
        switch (nextSegmentDirection)
        {
            case Direction.up:
                spriteRenderer.sprite = aboutToGetCoffee ? SpriteController.sharedInstance.upBiteSprite : SpriteController.sharedInstance.upHeadSprite;
                break;
            case Direction.left:
                spriteRenderer.sprite = aboutToGetCoffee ? SpriteController.sharedInstance.leftBiteSprite : SpriteController.sharedInstance.leftHeadSprite;
                break;
            case Direction.right:
                spriteRenderer.sprite = aboutToGetCoffee ? SpriteController.sharedInstance.rightBiteSprite : SpriteController.sharedInstance.rightHeadSprite;
                break;
            case Direction.down:
                spriteRenderer.sprite = aboutToGetCoffee ? SpriteController.sharedInstance.downBiteSprite : SpriteController.sharedInstance.downHeadSprite;
                break;
        }
    }

    protected override void MoveTo(Vector2 newPosition)
    {
        if (addSegmentOnNextMove)
        {
            addSegmentOnNextMove = false;

            Vector2 oldPosition = transform.position;
            rigidBody.MovePosition(newPosition);


            SnakeSegment newSegment = BoardController.sharedInstance.NewSnakeSegment(oldPosition);
            newSegment.nextSegment = nextSegment;
            newSegment.nextSegment.previousSegment = newSegment;
            newSegment.previousSegment = this;
            nextSegment = newSegment;

            newSegment.previousSegmentDirection = movementController.currentDirection;
            Direction nextDirection = GetDirection(oldPosition, newSegment.nextSegment.transform.position);
            newSegment.UpdateSprite(nextDirection);
        }
        else
        {
            base.MoveTo(newPosition);
        }
        BoardController.sharedInstance.SnakeEnteredPosition(newPosition);
        UpdateSprite(movementController.currentDirection);
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

    private Vector2 PositionAfterMoving(int numberOfMoves)
    {
        float totalMovementAmount = movementAmount * numberOfMoves;
        Vector2 newPosition = transform.position;
        switch (movementController.currentDirection)
        {
            case Direction.up:
                newPosition.y += totalMovementAmount;
                break;
            case Direction.left:
                newPosition.x -= totalMovementAmount;
                break;
            case Direction.right:
                newPosition.x += totalMovementAmount;
                break;
            case Direction.down:
                newPosition.y -= totalMovementAmount;
                break;
        }
        return newPosition;
    }

    private void UpdateSprite()
    {

    }
}
