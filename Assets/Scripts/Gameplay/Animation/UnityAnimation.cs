using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class UnityAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    Tween scaleTween;
    Vector3 full = new Vector3(1, 1, 1);
    Vector3 distore = new Vector3(0.9f, 1.1f, 0);
    void Start()
    {
        scaleTween = transform.DOScale(distore,1.0f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
