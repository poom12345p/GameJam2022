using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class RewardUI : BaseUIAnimator
{
    [SerializeField] TextMeshProUGUI moveText;
    [SerializeField] Image trophy;

    public override void Show(Action _onComplete = null)
    {
        base.Show(_onComplete);
    }

    public void ShowReward(int _minimum, int _move)
    {
        if (_move <= _minimum) trophy.enabled = true;
        else trophy.enabled = false;
        UpdateSave(_minimum, _move);
    }

    public void UpdateSave(int _minimum, int _move)
    {
        var _currentScene = SceneManager.GetActiveScene();
        if (_move < GameManager.Instance.GetInt(_currentScene.name))
        {
            GameManager.Instance.SetInt(_currentScene.name, _move);
        }
        if (_currentScene.buildIndex > GameManager.Instance.GetUnlockedLevel())
        {
            GameManager.Instance.SetUnlockedLevel(_currentScene.buildIndex);
        }
    }

    public void UpdateMoveText(int _move)
    {
        moveText.text = "Move Count : " + _move;
    }

    public void Retry_Btn()
    {
        SceneLoader.Instance.ReloadScene();
    }

    public void Next_Btn()
    {
        SceneLoader.Instance.LoadNextScene();
    }
}
