using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Snake : MonoBehaviour
{
    [SerializeField]
    private int length = 5;

    public static Snake instance;
    public Snakesegment snakePrefab;
    public Snakesegment _head;

    private Vector2Int _direction;
    private Vector2Int _newDirection;
    private Vector2Int _headPosition;
    private Vector2Int[] _headHistory;
    private int[] _directionHistory;

    bool reverse = false;
    bool _reverseFired = false;
    private int _snekDelta = 0;

    private List<Snakesegment> _segments;
    public List<Snakesegment> Segments
    {
        set => _segments = value;
        get => _segments;
    }
    private List<Edible> _edibles;
    public List<Edible> Edibles
    {
        set => _edibles = value;
        get => _edibles;
    }

    public UnityEvent onSnakeScore;
    public UnityEvent onSnakeDead;
    private Vector2Int GetVector(float angle)
    {
        angle *= Mathf.Deg2Rad;
        var vector= new  Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        return new Vector2Int((int)vector.x, (int)vector.y);
    }

    private void Purge()
    {
        for (int i = 0; i < _segments.Count; i++)
            Destroy(_segments[i].gameObject);
      
        for (int i = 0; i < _edibles.Count; i++)
            Destroy(_edibles[i].gameObject);

        _segments.Clear();
        _edibles.Clear();

    }

    public void Init()
    {
        length = 5;
        _direction = Vector2Int.right;
        _headPosition = Playspace.instance.tileDimensions / 2;
        _headHistory = new Vector2Int[length];
        _directionHistory = new int[length];

        for (int i = 0; i < length; i++)
        {
            _segments.Add(Instantiate(snakePrefab, transform));
            _headHistory[length - 1 - i] = _headPosition + Vector2Int.left * (1 + i);
            _directionHistory[length - 1 - i] = 0;
        }
        _newDirection = _direction;
    }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        if (!Playspace.instance)
            return;

        _segments = new List<Snakesegment>();
        _edibles = new List<Edible>();
        GameCore.OnGameTick += UpdatePosition;
        GameCore.OnGameTick += CheckEdibles;

        Init();

    }

    int GetRotation()
    {
        int angle = 0;
        if (_direction.x == -1)
            angle = 180;
        if (_direction.y != 0)
            angle = _direction.y * 90;

        return angle;
    }

    bool CheckCollision()
    {
        foreach (var position in _headHistory)
            if(position.Equals(_headPosition))
            return true;
        return false;
    }

    void DrawSnek()
    {
        for (int i = 0; i < length; i++)
        {
            _segments[i].Position = _headHistory[length - i - 1];
            _segments[i].Direction = _directionHistory[length - i - 1];
            _segments[i]?.Draw();
        }
        _head.Draw();
    }

    void UpdatePosition()
    {
        if (CheckCollision())
        {
            onSnakeDead.Invoke();
            Purge();
            Init();
            return;
        }
        _direction = _newDirection;

        if (_snekDelta < 0)
        {
            SnakeReduce();
            _snekDelta++;
        }
        //lower index = closer to head
        Array.Copy(_directionHistory, 1, _directionHistory, 0, length - 1);
        _directionHistory[length-1] = GetRotation();

        Array.Copy(_headHistory, 1, _headHistory, 0, length - 1);
        _headHistory[length - 1] = _headPosition;

        _headPosition += _direction;

        if (_headPosition.x < 1)
        {
            _headPosition.x = Playspace.instance.tileDimensions.x;
        }
        else if (_headPosition.x > Playspace.instance.tileDimensions.x)
        {
            _headPosition.x = 1;
        }

        if (_headPosition.y < 0)
        {
            _headPosition.y = Playspace.instance.tileDimensions.y-1;
        }
        else if (_headPosition.y > Playspace.instance.tileDimensions.y-1 )
        {
            _headPosition.y = 0;
        }

        _head.Position = _headPosition;
        DrawSnek();

        if (_snekDelta > 0)
        {
            SnakeExtend();
            _snekDelta--;
        }

        _reverseFired = false;
    }

    void SnakeExtend()
    {
        length++;

        var _pos = _headHistory.ToList();
        var _rot = _directionHistory.ToList();

        _pos.Insert(0, _headHistory[0] + GetVector(_directionHistory[0]));
        _rot.Insert(0, _directionHistory[0]);

        _headHistory = _pos.ToArray();
        _directionHistory = _rot.ToArray();

        Snakesegment segment;

        if (_segments.Count < length)
        {
            segment = Instantiate(snakePrefab, transform);
            _segments.Add(segment);
        }
        else
        {
            segment = _segments[length-1];
            segment.gameObject.SetActive(true);
        }

        segment.transform.localPosition = _segments[length-1].transform.localPosition;
        segment.transform.localRotation = _segments[length-1].transform.localRotation;
    }
    void SnakeReduce()
    {
        if(length < 5 )
        {
            _snekDelta = 0;
            return;
        }
        length--;

        var _pos = _headHistory.ToList();
        var _rot = _directionHistory.ToList();

        _pos.RemoveAt(0);
        _rot.RemoveAt(0);

        _headHistory = _pos.ToArray();
        _directionHistory = _rot.ToArray();

        for(int i = length; i < _segments.Count; i++)
        {
            _segments[i].gameObject.SetActive(false);
        }
    }
    
    void Reverse()
    {
        if (_reverseFired)
            return;
        else
            _reverseFired = true;

        _direction = GetVector(_directionHistory[0]);
        _newDirection = -_direction;
        _headPosition = _headHistory[0] - _direction;
        _head.Position = _headPosition;
        
        Array.Reverse(_headHistory);
        Array.Reverse(_directionHistory);
        DrawSnek();
    }

    IEnumerator TemporaryTimechange(float change, float duration)
    {
        GameCore.instance.Delay = change;
        yield return new WaitForSeconds(duration);
        GameCore.instance.Delay = 0;
    }
    
    void CheckEdibles()
    {
        for(int i = 0; i < _edibles.Count; i++)
        {
            if ((_edibles[i].transform.localPosition - _head.transform.localPosition).magnitude < 0.1f)
            {
                switch (_edibles[i].type)
                {
                    case EdibleType.Reverse:
                        Reverse();
                        reverse = true;
                        break;

                    case EdibleType.Short:
                        if(length>5)
                            _snekDelta--;
                        break;

                    case EdibleType.Turbo:
                        StartCoroutine(TemporaryTimechange(-0.1f, 7f));
                        break;

                    case EdibleType.Slow:
                        StartCoroutine(TemporaryTimechange(0.1f, 7f));
                        break;

                    default:
                        _snekDelta++;
                        break;
                }
                onSnakeScore.Invoke();
                Destroy(_edibles[i].gameObject);
                _edibles.RemoveAt(i);
                return;
            }
        }
    }
    
    
    // Update is called once per frame
    void LateUpdate()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("biggur snek");
            _snekDelta++;
        }
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("smoller snek");
            _snekDelta--;
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("reverse snek");
            Reverse();
            reverse = true;
        }
#endif


        if (Math.Abs(_direction.y) == 0)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                _newDirection = Vector2Int.up;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                _newDirection = Vector2Int.down;
            }
        }
        else if (Math.Abs(_direction.x) == 0)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                _newDirection = Vector2Int.left;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                _newDirection = Vector2Int.right;
            }
        }
    }


}
