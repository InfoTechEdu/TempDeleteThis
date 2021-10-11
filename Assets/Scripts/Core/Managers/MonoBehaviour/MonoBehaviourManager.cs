using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonoBehaviourManager<T> : Singleton<T> where T : MonoBehaviour
{
    [SerializeField] protected GameDataStorage gameDataStorage;

    protected bool isStarted;

    protected virtual void Awake()
    {
        if (!isStarted)
            DontDestroyOnLoad(gameObject);
    }
}
