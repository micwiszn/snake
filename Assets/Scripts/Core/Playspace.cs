using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playspace : MonoBehaviour
{
    public static Playspace instance;
    private RectTransform _rect;

    public Vector2Int tileDimensions = new Vector2Int(20,20);
    [SerializeField]
    private int tileSize = 20;

    public int TileSize
    {
        get => tileSize;
    }

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        _rect = GetComponent<RectTransform>();
        _rect.sizeDelta = tileSize * tileDimensions;
    }

    public Vector2 PlayspaceToLocal(Vector2Int position)
    {
        return PlayspaceToLocal(position.x, position.y);
    }
    public Vector2 PlayspaceToLocal(int x, int y) 
    {
        float posX = (x - tileDimensions.x/2) * tileSize;
        float posY = (y - tileDimensions.y/2) * tileSize;
        return new Vector2(posX,posY);
    }
}
