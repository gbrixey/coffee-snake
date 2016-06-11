using UnityEngine;
using System.Collections;
using Direction = MovementController.Direction;
using SnakeHeadSpriteType = SpriteController.SnakeHeadSpriteType;

/// <summary>
/// The most important part of the snake.
/// </summary>
public class SnakeHeadSegment : SnakeSegment
{
    public MovementController movementController;

    private const float movementAmount = 1.0f;
    private bool addSegmentOnNextMove = false;

    protected override void Awake()
    {
        base.Awake();
        movementController = GetComponent<MovementController>();
    }

    void FixedUpdate()
    {
        if (GameController.sharedInstance.canMove)
        {
            movementController.UpdateDirection();
            AttemptMove();
        }
    }

    private void OnDisable()
    {
        // Update to the dead snake sprite. Don't use the movement direction for this.
        // The snake may have tried to move in a new direction, but it died and the movement 
        // was not carried out. Use the opposite direction of the next snake segment,
        // which corresponds to the previous movement direction.
        Direction movementDirection = MovementController.GetDirection(nextSegment.transform.position, transform.position);
        spriteRenderer.sprite = SpriteController.sharedInstance.GetSnakeHeadSprite(movementDirection, SnakeHeadSpriteType.dead);
    }

    /// <summary>
    /// Updates the snake head sprite based on the direction it is moving.
    /// </summary>
    public override void UpdateSprite()
    {
        // Show the snake opening its mouth if it is about to get the coffee.
        RaycastHit2D hit = Physics2D.Linecast(transform.position, GetNextPosition());
        bool aboutToGetCoffee = (hit.transform != null && hit.transform.GetComponent<Coffee>() != null);
        SnakeHeadSpriteType type = aboutToGetCoffee ? SnakeHeadSpriteType.biting : SnakeHeadSpriteType.normal;
        spriteRenderer.sprite = SpriteController.sharedInstance.GetSnakeHeadSprite(movementController.currentDirection, type);
    }

    /// <summary>
    /// Moves the snake head to the given position.
    /// Adds a new snake segment if necessary.
    /// </summary>
    protected override void MoveTo(Vector2 newPosition)
    {
        if (addSegmentOnNextMove)
        {
            // Move only the head and not the other segments.
            // Add the new segment right behind the head.
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

    /// <summary>
    /// Attempts to move in the current direction and
    /// interacts with any game objects that are in the way.
    /// </summary>
    private void AttemptMove()
    {
        Vector2 newPosition = GetNextPosition();
        RaycastHit2D hit = Physics2D.Linecast(transform.position, newPosition);

        if (hit.transform != null)
        {
            // The snake cannot move onto snake segments or walls,
            // so call GameOver and do not complete the move.
            if (hit.transform.GetComponent<SnakeSegment>() != null ||
                hit.transform.GetComponent<Wall>() != null)
            {
                GameController.sharedInstance.GameOver();
            }
            else
            {
                // Complete the move.
                MoveTo(newPosition);

                // If the snake just moved onto coffee, drink the coffee.
                // This needs to be done after the move is completed to ensure
                // that we add two snake segments if the snake drinks two coffees in a row.
                Coffee coffee = hit.transform.GetComponent<Coffee>();
                if (coffee != null)
                {
                    GameController.sharedInstance.IncrementScore();
                    movementController.IncreaseMovementSpeed();
                    addSegmentOnNextMove = true;
                    // There is only ever one coffee, so rather than destroying
                    // it and creating a new one, we can just move the existing coffee.
                    BoardController.sharedInstance.MoveCoffee();
                }
            }
        }
        else
        {
            MoveTo(newPosition);
        }
    }

    /// <summary>
    /// Gets the next position the snake will move to
    /// if the snake moves in the current direction.
    /// </summary>
    private Vector2 GetNextPosition()
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
