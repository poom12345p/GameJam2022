using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    SceneLoader sceneLoader;
    [SerializeField] AudioClip audioClip;

    private void Awake()
    {
        base.Awake();
        sceneLoader = SceneLoader.Instance;
    }
}
