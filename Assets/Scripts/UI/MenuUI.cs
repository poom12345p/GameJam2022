using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MenuUI : BaseUIAnimator
{
    [SerializeField] OptionUI optionUI;

    public void Btn_Play()
    {
        SceneLoader.Instance.LoadScene(GameManager.Instance.UnlockedLevel + 1);

    }

    public void Btn_LevelSelect()
    {
        SceneLoader.Instance.LoadLevelSelectScene();
    }

    public void Btn_Option()
    {
        optionUI.Show();
    }

    public void Btn_Exit()
    {
        Action _onComplete = QuitGame;
        SceneLoader.Instance.TransitionUI.FadeIn(_onComplete);
    }

    void QuitGame()
    {
        Application.Quit();
    }

    public void Btn_MainMenu()
    {
        SceneLoader.Instance.LoadMenuScene();
    }
}
