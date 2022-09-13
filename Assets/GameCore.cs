using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCore : MonoBehaviour
{
    public float timeframe = 0.25f;
    [SerializeField]
    private bool _gameRun = false;

    public delegate void GameTick();
    public static event GameTick OnGameTick;

    private void Start()
    {
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
            yield return new WaitForSeconds(timeframe);
            if (OnGameTick != null)
                OnGameTick();
        }
    }
}
