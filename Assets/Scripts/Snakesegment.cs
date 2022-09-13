using UnityEngine;

public class Snakesegment : MonoBehaviour
{
    [SerializeField]
    private Vector2Int _position;
    [SerializeField]
    private int _direction;

    private RectTransform _rect;

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

    //void OnEnable()
    void Start()
    {
        _rect = GetComponent<RectTransform>();
        _rect.sizeDelta = Vector2.one * Playspace.instance.TileSize;
    }

    public void Draw()
    {
        _rect.transform.localPosition = Playspace.instance.PlayspaceToLocal(_position.x, _position.y);
        _rect.transform.localEulerAngles = Vector3.forward*_direction;
    }
}
