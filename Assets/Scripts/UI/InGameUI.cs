using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class InGameUI : BaseUIAnimator
{
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI moveText;

    private void Awake()
    {
        SetLevel(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void SetLevel(int _level)
    {
        levelText.text = "Stage : " + _level.ToString();
    }

    public void SetMove(int _move)
    {
        moveText.text = "Move used : " + _move.ToString();
    }

}
