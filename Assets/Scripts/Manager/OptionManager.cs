using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionManager : Singleton<OptionManager>
{
    public float BGMVolume { get => bgmVolume; set => bgmVolume = value; }
    public float SFXVolume { get => sfxVolume; set => sfxVolume = value; }

    [SerializeField, Range(0, 1)] float bgmVolume = 1f;
    [SerializeField, Range(0, 1)] float sfxVolume = 1f;
}
