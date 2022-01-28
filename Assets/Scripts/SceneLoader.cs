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

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //}
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        //}
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
