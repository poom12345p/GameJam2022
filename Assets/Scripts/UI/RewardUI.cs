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
    [SerializeField] AudioClip victoryClip;

    public override void Show(Action _onComplete = null)
    {
        base.Show(_onComplete);
        SoundManager.Instance.SfxPlay(victoryClip);
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
        if (GameManager.Instance.GetInt(_currentScene.name) == 0 || _move < GameManager.Instance.GetInt(_currentScene.name))
        {
            if (_move <= _minimum)  GameManager.Instance.SetInt(_currentScene.name + "_Trophy", 1);
            GameManager.Instance.SetInt(_currentScene.name, _move);
        }
        if (_currentScene.buildIndex > GameManager.Instance.GetUnlockedLevel()) GameManager.Instance.SetUnlockedLevel(_currentScene.buildIndex);
    }

    public void UpdateMoveText(int _move)
    {
        moveText.text = "Move Count : " + _move;
    }

    public void Btn_Retry()
    {
        SceneLoader.Instance.ReloadScene();
    }

    public void Btn_Next()
    {
        SceneLoader.Instance.LoadNextScene();
    }
}
