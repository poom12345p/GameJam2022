using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : BaseUIAnimator
{
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] AudioClip sfxClip;

    public override void Show(Action _onComplete = null)
    {
        bgmSlider.value = OptionManager.Instance.BGMVolume;
        sfxSlider.value = OptionManager.Instance.SFXVolume;
        base.Show(_onComplete);
    }

    public void UpdateBGMVolume()
    {
        OptionManager.Instance.UpdateBGMVolume(bgmSlider.value);
    }

    public void UpdateSFXVolume()
    {
        OptionManager.Instance.UpdateSFXVolume(sfxSlider.value);
    }

    public void PlaySampleSFX()
    {
        SoundManager.Instance.SfxPlay(sfxClip);
    }

    public void Btn_Back()
    {
        base.Hide();
    }

    public void Btn_DeleteSave()
    {
        GameManager.Instance.DeleteSave();
    }
}
