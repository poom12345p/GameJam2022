using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class RewardUI : BaseUIAnimator
{
    [SerializeField] TextMeshProUGUI moveText;

    public override void Show(Action _onComplete = null)
    {
        base.Show(_onComplete);
    }

    void UpdateMoveText(int _move)
    {
        moveText.text = "Move Count : " + _move;
    }
}
