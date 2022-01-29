using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int UnlockedLevel { get => unlockedLevel; set => unlockedLevel = value; }

    private int unlockedLevel = 1;

    [SerializeField] AudioClip bgmClip;

    private void Awake()
    {
        base.Awake();
        SoundManager.Instance.BgmPlay(bgmClip);
    }
}
