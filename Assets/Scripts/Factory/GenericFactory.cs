using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericFactory<T> : MonoBehaviour where T : MonoBehaviour
{
    // Reference to prefab
    [SerializeField]
    private T prefab;

    public T GetNewInstance()
    {
        return Instantiate(prefab,transform);
    }
}
