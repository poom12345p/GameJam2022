using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using System;

public class BaseAnimator : MonoBehaviour
{
    public bool IsInterruptible { get => isInterruptible; set => isInterruptible = value; }


    [SerializeField] protected Animator animator;
    [SerializeField] protected AnimationClip inClip;
    [SerializeField] protected AnimationClip outClip;
    [SerializeField] protected bool isInterruptible = false;

    [ReadOnly] protected bool isPlaying;

    public void PlayInClip(Action _onComplete = null)
    {
        if (isInterruptible && isPlaying) return;
        StartCoroutine(iePlayInClip(_onComplete));
    }

    public void PlayOutClip(Action _onComplete = null)
    {
        if (isInterruptible && isPlaying) return;
        StartCoroutine(iePlayOutClip(_onComplete));
    }

    public IEnumerator iePlayInClip(Action _onComplete = null)
    {
        isPlaying = true;
        animator.SetTrigger("in");
        if (inClip != null) yield return new WaitForSeconds(inClip.length);
        isPlaying = false;
        _onComplete?.Invoke();
    }

    public IEnumerator iePlayOutClip(Action _onComplete = null)
    {
        isPlaying = true;
        animator.SetTrigger("out");
        if (outClip != null) yield return new WaitForSeconds(outClip.length);
        isPlaying = false;
        _onComplete?.Invoke();
    }
}
