using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    // Spawn rate
    [SerializeField]
    private float spawnRate = 12;

    private float _currentRate = 0;
    private float _lastTime;

    // Current spawn count
    private int currentCount = 0;
    public void DecreaseCount()
    {
        if(currentCount > 0)
            currentCount--;
    }

    // Total spawn count
    [SerializeField]
    private int targetCount = 3;

    // Reference to factory
    [SerializeField]
    private TimedFactory factory;
    Vector2Int _playspaceSize;

    private void Start()
    {
        _playspaceSize = Playspace.instance.tileDimensions;
        _lastTime = Time.time;
    }
    public void Spawn()
    {
        _currentRate = currentCount / (Time.time-_lastTime);
        if (targetCount > currentCount && _currentRate < spawnRate)
        {
            var spawn = factory.GetNewInstance();
            var randomPos = new Vector2Int(Random.Range(1, _playspaceSize.x - 1), Random.Range(1, _playspaceSize.y - 1));
  
            spawn.transform.localPosition = Playspace.instance.PlayspaceToLocal(randomPos);
            spawn.Spawner = this;

            _lastTime = Time.time;
            currentCount++;
        }
    }
}