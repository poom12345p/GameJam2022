using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

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
        if(_move <= _minimum)
        {
            trophy.enabled = true;
        }
        else
        {
            trophy.enabled = false;
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
