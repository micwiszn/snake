using System;
using UnityEngine;

public class Snakesegment : MonoBehaviour
{
    [SerializeField]
    private Vector2Int _position;
    [SerializeField]
    private int _direction;

    private RectTransform _rect;
    public RectTransform Rect
    {
        get => _rect;
    }

    public Vector2Int Position
    {
        get => _position;
        set => _position = value;
    }
    public int Direction
    {
        get => _direction;
        set => _direction = value;
    }
    private void OnDisable()
    {
        _position = Vector2Int.one * 1000;
        _rect.transform.localPosition = Playspace.instance.PlayspaceToLocal(_position.x, _position.y);
        _rect.transform.localEulerAngles = Vector3.forward * _direction;
    }

    void Start()
    {
        _rect = GetComponent<RectTransform>();
        _rect.sizeDelta = Vector2.one * Playspace.instance.TileSize;
        gameObject.SetActive(false);
    }

    public void Draw()
    {
        gameObject.SetActive(true);
        //_rect.transform.localEulerAngles = Vector3.forward*_direction;
        //_rect.transform.localPosition = Playspace.instance.PlayspaceToLocal(_position.x, _position.y);
    }
}
