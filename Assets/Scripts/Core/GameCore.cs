using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameCore : MonoBehaviour
{
    public static GameCore instance;
    [SerializeField]
    private float _timeframe = 0.25f;
    public float Timeframe
    {
        get => _timeframe;
    }

    [SerializeField]
    private float _extraDelay = 0;

    public float Delay
    {
        set
        {
            if (value < _timeframe)
                _extraDelay = value;
            else
                _extraDelay = _timeframe * 0.5f;
        }
        get => _extraDelay;
    }

    private bool _gameRun = false;

    public delegate void GameTick();
    public static event GameTick OnGameTick;

    public UnityEvent onGameTick;

    private void Start()
    {
        instance = this;
        StartClock();
    }

    public void StartClock()
    {
        if(!_gameRun)
            StartCoroutine(GameClock());
    }

    IEnumerator GameClock()
    {
        _gameRun = true;
        while (_gameRun)
        {
            yield return new WaitForSeconds(_timeframe + _extraDelay);
            if (OnGameTick != null)
            {
                OnGameTick();
                onGameTick.Invoke();
            }
        }
    }
}
