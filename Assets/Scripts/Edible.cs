using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EdibleType
{
    Long = 0,
    Short = 1,
    Reverse = 2
}

public class Edible : MonoBehaviour
{
    public EdibleType type;
    private Snake snake;
    private Vector2Int position;

    private void OnEnable()
    {
        snake = Snake.instance;
        var range = Random.Range(0, 30);

        if (range < 2)
        {
            type = EdibleType.Reverse;
        }
        else if (range < 10)
            type = EdibleType.Short;
        else
            type = EdibleType.Long;

        type = (EdibleType)Random.Range(0, 3);

        snake.edibles.Add(this);
    }

    private void OnDestroy()
    {
        snake.edibles.Remove(this);
    }
}
