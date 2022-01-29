using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class SceneLoader : Singleton<SceneLoader>
{
    [SerializeField] TransitionUI transitionUI;

    private void Awake()
    {
        base.Awake();
    }

    public void LoadScene(int _index)
    {
        StartCoroutine(ieLoadScene(_index));
    }

    public IEnumerator ieLoadScene(int _index, Action _onComplete = null)
    {
        yield return transitionUI.ieFadeIn();
        SceneManager.LoadScene(_index);
        yield return transitionUI.ieFadeOut();
        _onComplete?.Invoke();
    }
}
