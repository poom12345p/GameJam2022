using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance { get => CreateInstance(); }

    static T CreateInstance()
    {
        instance = FindObjectOfType<T>();
        if (instance == null)
        {
            instance = new GameObject("[Singleton] " + typeof(T).ToString()).AddComponent<T>();
            Debug.Log("[Singleton] Create new instance of " + typeof(T));
        }
        return instance;
    }

    public virtual void Awake()
    {
        if(this != Instance)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this);
        }
    }
}