using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EpicSaxPlayer : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    private void Awake()
    {
        SoundManager.Instance.BgmAudioSource.Stop();
        audioSource.volume *= OptionManager.Instance.BGMVolume;
        audioSource.Play();
    }
}
