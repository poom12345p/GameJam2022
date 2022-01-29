using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InGameUI : BaseUIAnimator
{
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI moveText;

    public void SetLevel(int _level)
    {
        levelText.text = "Stage : " + _level.ToString();
    }

    public void SetMove(int _move)
    {
        moveText.text = "Move used : " + _move.ToString();
    }

}
