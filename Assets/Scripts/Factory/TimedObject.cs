using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedObject : MonoBehaviour
{
    private PrefabSpawner spawner;
    public PrefabSpawner Spawner
    {
        set => spawner = value;
    }

    // Lifespan
    [SerializeField]
    private float timeout = 2;

    // Spawning time
    private float startTime;

    void OnEnable()
    {
        startTime = Time.time;
    }

    private void OnDestroy()
    {
        spawner?.DecreaseCount();
    }

    void Update()
    {
        // Countdown
        if (Time.time - startTime > timeout)
        {
            // Destroy
            Destroy(gameObject);
        }
    }
}
