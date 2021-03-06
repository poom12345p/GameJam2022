using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class SceneLoader : Singleton<SceneLoader>
{
    public TransitionUI TransitionUI { get => transitionUI; set => transitionUI = value; }
    public bool IsLoading { get => isLoading; set => isLoading = value; }

    [SerializeField] TransitionUI transitionUI;
    [SerializeField] int loadCount = 0;

    bool isLoading;

    private void Awake()
    {
        base.Awake();
    }

    public void LoadMenuScene()
    {
        LoadScene(0);
    }

    public void LoadLevelSelectScene()
    {
        LoadScene(1);
    }

    public void ReloadScene()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextScene()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadScene(int _index)
    {
        if (isLoading) return;
        StartCoroutine(ieLoadScene(_index));
        loadCount++;
    }

    public void LoadScene(string _sceneName)
    {
        if (isLoading) return;
        StartCoroutine(ieLoadScene(_sceneName));
        loadCount++;
    }

    public IEnumerator ieLoadScene(int _index, Action _onComplete = null)
    {
        isLoading = true;
        if (transitionUI != null) yield return transitionUI.ieFadeIn();
        SceneManager.LoadScene(_index);
        if (transitionUI != null) yield return transitionUI.ieFadeOut();
        isLoading = false;
        _onComplete?.Invoke();
    }

    public IEnumerator ieLoadScene(string _sceneName, Action _onComplete = null)
    {
        isLoading = true;
        if (transitionUI != null) yield return transitionUI.ieFadeIn();
        SceneManager.LoadScene(_sceneName);
        if (transitionUI != null) yield return transitionUI.ieFadeOut();
        isLoading = false;
        _onComplete?.Invoke();
    }
}
