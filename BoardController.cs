using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Controls the game board and all of the game objects
/// that occupy a physical position on the board.
/// </summary>
public class BoardController : MonoBehaviour
{
    public static BoardController sharedInstance;

    public int columns = 16;
    public int rows = 16;

    public SnakeHeadSegment snakeHead;
    public SnakeSegment snakeBody;
    public SnakeTailSegment snakeTail;
    public GameObject wall;
    public GameObject coffee;

    // These transforms are to organize the game object hierarchy in Unity Editor.
    private Transform wallContainer;
    public Transform snakeSegmentContainer;

    private HashSet<Vector2> unoccupiedPositions = new HashSet<Vector2>();

    void Awake()
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
        }
        else if (sharedInstance != this)
        {
            Destroy(gameObject);
        }

        // Initialize set of unoccupied positions,
        // excluding permanently occupied positions (walls)
        for (int row = 1; row < rows + 1; row++)
        {
            for (int column = 1; column < columns + 1; column++)
            {
                unoccupiedPositions.Add(new Vector2(column, row));
            }
        }
    }

    private void OnDisable()
    {
        snakeHead.enabled = false;
    }

    /// <summary>
    /// Creates the wall objects that enclose the playable area.
    /// </summary>
    public void CreateWalls()
    {
        wallContainer = new GameObject("WallContainer").transform;
        GameObject wallInstance;

        // Left and right sides
        for (int row = 0; row < rows + 2; row++)
        {
            wallInstance = Instantiate(wall, new Vector3(0.0f, row, 0.0f), Quaternion.identity) as GameObject;
            wallInstance.transform.SetParent(wallContainer);

            wallInstance = Instantiate(wall, new Vector3(columns + 1.0f, row, 0.0f), Quaternion.identity) as GameObject;
            wallInstance.transform.SetParent(wallContainer);
        }
        // Top and bottom.
        // Avoid placing duplicate walls at the corners.
        for (int column = 1; column < columns + 1; column++)
        {
            wallInstance = Instantiate(wall, new Vector3(column, 0.0f, 0.0f), Quaternion.identity) as GameObject;
            wallInstance.transform.SetParent(wallContainer);

            wallInstance = Instantiate(wall, new Vector3(column, rows + 1.0f, 0.0f), Quaternion.identity) as GameObject;
            wallInstance.transform.SetParent(wallContainer);
        }
    }

    /// <summary>
    /// Creates the snake. The snake starts with 6 segments.
    /// </summary>
    public void CreateSnake()
    {
        snakeSegmentContainer = new GameObject("SnakeSegmentContainer").transform;

        Vector3 position = new Vector3(7.0f, (rows + 2.0f) / 2.0f, 0.0f);
        snakeHead = Instantiate(snakeHead, position, Quaternion.identity) as SnakeHeadSegment;
        snakeHead.transform.SetParent(snakeSegmentContainer);
        SnakeEnteredPosition(position);
        position.x -= 1.0f;
        SnakeSegment snakeBody1 = NewSnakeSegment(position);
        position.x -= 1.0f;
        SnakeSegment snakeBody2 = NewSnakeSegment(position);
        position.x -= 1.0f;
        SnakeSegment snakeBody3 = NewSnakeSegment(position);
        position.x -= 1.0f;
        SnakeSegment snakeBody4 = NewSnakeSegment(position);
        position.x -= 1.0f;
        snakeTail = Instantiate(snakeTail, position, Quaternion.identity) as SnakeTailSegment;
        snakeTail.transform.SetParent(snakeSegmentContainer);
        SnakeEnteredPosition(position);

        snakeHead.nextSegment = snakeBody1;
        snakeBody1.previousSegment = snakeHead;
        snakeBody1.nextSegment = snakeBody2;
        snakeBody2.previousSegment = snakeBody1;
        snakeBody2.nextSegment = snakeBody3;
        snakeBody3.previousSegment = snakeBody2;
        snakeBody3.nextSegment = snakeBody4;
        snakeBody4.previousSegment = snakeBody3;
        snakeBody4.nextSegment = snakeTail;
        snakeTail.previousSegment = snakeBody4;

        snakeHead.enabled = true;
    }

    /// <summary>
    /// Creates a new snake body segment at the given position.
    /// </summary>
    public SnakeSegment NewSnakeSegment(Vector3 position)
    {
        SnakeSegment newSegment = Instantiate(snakeBody, position, Quaternion.identity) as SnakeSegment;
        newSegment.transform.SetParent(snakeSegmentContainer);
        SnakeEnteredPosition((Vector2)position);
        return newSegment;
    }

    /// <summary>
    /// Initializes the coffee and places it somewhere on the board.
    /// This should be called after CreateSnake to avoid placing the coffee
    /// in the snake's starting position.
    /// </summary>
    public void CreateCoffee()
    {
        Instantiate(coffee, new Vector3(1.0f, 1.0f, 1.0f), Quaternion.identity);
        MoveCoffee();
    }

    /// <summary>
    /// Moves the coffee to a random unoccupied position on the board.
    /// </summary>
    public void MoveCoffee()
    {
        GameObject coffeeToMove = GameObject.FindGameObjectWithTag("Coffee");
        Vector2 oldPosition = coffeeToMove.transform.position;
        int count = unoccupiedPositions.Count;
        Vector2[] unoccupiedPositionsArray = new Vector2[count];
        unoccupiedPositions.CopyTo(unoccupiedPositionsArray);
        // Use count - 2 to avoid array out of bounds errors.
        Vector2 newPosition = unoccupiedPositionsArray[Random.Range(0, count - 2)];
        unoccupiedPositions.Remove(newPosition);
        unoccupiedPositions.Add(oldPosition);
        coffeeToMove.transform.position = newPosition;
    }

    /// <summary>
    /// Notifies the board controller that the snake has
    /// occupied the given position.
    /// </summary>
    public void SnakeEnteredPosition(Vector2 position)
    {
        unoccupiedPositions.Remove(position);
    }

    /// <summary>
    /// Notifies the board controller that the snake has
    /// stopped occupying the given position.
    /// </summary>
    public void SnakeLeftPosition(Vector2 position)
    {
        unoccupiedPositions.Add(position);
    }
}
