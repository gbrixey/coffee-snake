using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections;
using System.Collections.Generic;

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
        // Top and bottom
        for (int column = 1; column < columns + 1; column++)
        {
            wallInstance = Instantiate(wall, new Vector3(column, 0.0f, 0.0f), Quaternion.identity) as GameObject;
            wallInstance.transform.SetParent(wallContainer);

            wallInstance = Instantiate(wall, new Vector3(column, rows + 1.0f, 0.0f), Quaternion.identity) as GameObject;
            wallInstance.transform.SetParent(wallContainer);
        }
    }

    public void InitializeSnake()
    {
        snakeSegmentContainer = new GameObject("SnakeSegmentContainer").transform;

        Vector3 position = new Vector3(7.0f, (rows + 2.0f) / 2.0f, 0.0f);
        snakeHead = Instantiate(snakeHead, position, Quaternion.identity) as SnakeHeadSegment;
        snakeHead.transform.SetParent(snakeSegmentContainer);
        SnakeEnteredPosition(position);
        position.x -= 1.0f;
        SnakeSegment snakeBody1 = Instantiate(snakeBody, position, Quaternion.identity) as SnakeSegment;
        snakeBody1.transform.SetParent(snakeSegmentContainer);
        SnakeEnteredPosition(position);
        position.x -= 1.0f;
        SnakeSegment snakeBody2 = Instantiate(snakeBody, position, Quaternion.identity) as SnakeSegment;
        snakeBody2.transform.SetParent(snakeSegmentContainer);
        SnakeEnteredPosition(position);
        position.x -= 1.0f;
        SnakeSegment snakeBody3 = Instantiate(snakeBody, position, Quaternion.identity) as SnakeSegment;
        snakeBody3.transform.SetParent(snakeSegmentContainer);
        SnakeEnteredPosition(position);
        position.x -= 1.0f;
        SnakeSegment snakeBody4 = Instantiate(snakeBody, position, Quaternion.identity) as SnakeSegment;
        snakeBody4.transform.SetParent(snakeSegmentContainer);
        SnakeEnteredPosition(position);
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

    public SnakeSegment NewSnakeSegment(Vector3 position)
    {
        SnakeSegment newSegment = Instantiate(BoardController.sharedInstance.snakeBody, position, Quaternion.identity) as SnakeSegment;
        newSegment.transform.SetParent(snakeSegmentContainer);
        SnakeEnteredPosition((Vector2)position);
        return newSegment;
    }

    public void PlaceCoffee()
    {
        Instantiate(coffee, new Vector3(1.0f, 1.0f, 1.0f), Quaternion.identity);
        MoveCoffee();
    }

    public void MoveCoffee()
    {
        GameObject coffeeToMove = GameObject.FindGameObjectWithTag("Coffee");
        Vector2 oldPosition = coffeeToMove.transform.position;
        int count = unoccupiedPositions.Count;
        Vector2[] unoccupiedPositionsArray = new Vector2[count];
        unoccupiedPositions.CopyTo(unoccupiedPositionsArray);
        Vector2 newPosition = unoccupiedPositionsArray[Random.Range(0, count - 2)];
        unoccupiedPositions.Remove(newPosition);
        unoccupiedPositions.Add(oldPosition);
        coffeeToMove.transform.position = newPosition;
    }

    public void SnakeEnteredPosition(Vector2 position)
    {
        unoccupiedPositions.Remove(position);
    }

    public void SnakeLeftPosition(Vector2 position)
    {
        unoccupiedPositions.Add(position);
    }
}
