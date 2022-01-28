using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TransitionUI : BaseAnimator
{
    public void FadeIn(Action _onComplete = null)
    {
        base.Show(_onComplete);
    }

    public void FadeOut(Action _onComplete = null)
    {
        base.Hide(_onComplete);
    }

    public IEnumerator ieFadeIn(Action _onComplete = null)
    {
        if (MainCanvas.worldCamera == null) MainCanvas.worldCamera = Camera.main;
        yield return base.ieShow(_onComplete);
    }

    public IEnumerator ieFadeOut(Action _onComplete = null)
    {
        if (MainCanvas.worldCamera == null) MainCanvas.worldCamera = Camera.main;
        yield return base.ieHide(_onComplete);
    }
}
