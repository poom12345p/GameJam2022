using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : BaseUIAnimator
{
    [SerializeField] OptionUI optionUI;

    public PlayerUnit player;

    public override void Show(Action _onComplete = null)
    {
        if (isShow) return;
        if (player != null) player.canMove = false;
        base.Show(_onComplete);
    }

    public override void Hide(Action _onComplete = null)
    {
        if (!isShow) return;
        if (player != null) player.canMove = true;
        base.Hide(_onComplete);
    }

    public void Btn_Resume()
    {
        Hide();
    }

    public void Btn_Option()
    {
        optionUI.Show();
    }

    public void Btn_LevelSelect()
    {
        SceneLoader.Instance.LoadLevelSelectScene();
    }

    public void Btn_Menu()
    {
        SceneLoader.Instance.LoadMenuScene();
    }
}
