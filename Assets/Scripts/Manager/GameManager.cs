using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public int UnlockedLevel { get => unlockedLevel; set => unlockedLevel = value; }

    private int unlockedLevel = 1;

    [SerializeField] AudioClip bgmClip;

    private void Awake()
    {
        base.Awake();
        if (SoundManager.Instance.BgmAudioSource.clip == null) SoundManager.Instance.BgmPlay(bgmClip);
        if (GetUnlockedLevel() < 1) SetUnlockedLevel(1);
    }

    public void SetUnlockedLevel(int _value)
    {
        unlockedLevel = _value;
        SetInt("UnlockedLevel", _value);
    }

    public int GetUnlockedLevel()
    {
        unlockedLevel = GetInt("UnlockedLevel");
        return unlockedLevel;
    }

    public void SetInt(string _keyword, int _value)
    {
        PlayerPrefs.SetInt(_keyword, _value);
    }

    public int GetInt(string _keyword)
    {
        return PlayerPrefs.GetInt(_keyword);
    }
}
