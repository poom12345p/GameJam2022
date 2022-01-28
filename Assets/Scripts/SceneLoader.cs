using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class SceneLoader : BaseAnimator
{
    private void Awake()
    {
        if (FindObjectOfType<SceneLoader>() != this) Destroy(this.gameObject);
        DontDestroyOnLoad(this.gameObject);
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
        yield return iePlayInClip();
        SceneManager.LoadScene(_index);
        yield return iePlayOutClip();
        _onComplete?.Invoke();
    }
}
