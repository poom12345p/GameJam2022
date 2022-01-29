using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using System;

public class BaseUIAnimator : MonoBehaviour
{
    public Canvas MainCanvas { get => mainCanvas; set => mainCanvas = value; }
    public bool IsInterruptible { get => isInterruptible; set => isInterruptible = value; }
    public bool IsPlaying { get => isPlaying; set => isPlaying = value; }
    public bool IsShow { get => isShow; set => isShow = value; }

    [Header("Canvas Settings")]
    [SerializeField] private Canvas mainCanvas;

    [Header("Animator Settings")]
    [SerializeField] protected Animator animator;
    [SerializeField] protected AnimationClip inClip;
    [SerializeField] protected AnimationClip outClip;
    [SerializeField] protected bool isInterruptible = false;

    [ReadOnly] private bool isPlaying;
    [ReadOnly] protected bool isShow;

    private void Awake()
    {
        Debug.Log($"camera {Camera.main}");
        if (mainCanvas == null) mainCanvas = GetComponentInChildren<Canvas>();
        if (mainCanvas.worldCamera == null) mainCanvas.worldCamera = Camera.main;
    }

    public virtual void Show(Action _onComplete = null)
    {
        if (!isInterruptible && isPlaying) return;
        StartCoroutine(ieShow(_onComplete));
    }

    public virtual void Hide(Action _onComplete = null)
    {
        if (!isInterruptible && isPlaying) return;
        StartCoroutine(ieHide(_onComplete));
    }

    public virtual IEnumerator ieShow(Action _onComplete = null)
    {
        isPlaying = true;
        isShow = true;
        animator.SetTrigger("in");
        if (inClip != null) yield return new WaitForSeconds(inClip.length);
        isPlaying = false;
        _onComplete?.Invoke();
    }

    public virtual IEnumerator ieHide(Action _onComplete = null)
    {
        isPlaying = true;
        animator.SetTrigger("out");
        if (outClip != null) yield return new WaitForSeconds(outClip.length);
        isPlaying = false;
        isShow = false;
        _onComplete?.Invoke();
    }
}
