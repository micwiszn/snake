using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EdibleType
{
    Long = 0,
    Short = 1,
    Reverse = 2,
    Turbo = 3,
    Slow = 4
}

public class Edible : MonoBehaviour
{
    public EdibleType type;
    private Snake snake;
    private Vector2Int _position;
    public Vector2Int Position
    {
        set => _position = value;
        get => _position;
    }

    private void OnEnable()
    {
        snake = Snake.instance;
        var range = Random.Range(0, 50);

        //lowest chance for reverse
        if (range < 5)
        {
            type = EdibleType.Reverse;
        }
        //small chance for turbo
        else if (range < 8)
            type = EdibleType.Turbo;
        //small chance for slowdown
        else if (range < 16)
            type = EdibleType.Slow;
        //medium chance for shortening
        else if (range < 25)
            type = EdibleType.Short;
        //by default biggest chance for elongating
        else
            type = EdibleType.Long;

        //type = (EdibleType)Random.Range(0, 5);

        snake.Edibles.Add(this);
        name = type.ToString();
    }

    private void OnDestroy()
    {
        snake.Edibles.Remove(this);
    }
}
