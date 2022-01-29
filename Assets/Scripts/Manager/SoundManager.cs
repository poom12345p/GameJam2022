using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public AudioSource BgmAudioSource { get => bgmAudioSource; set => bgmAudioSource = value; }

    [SerializeField] AudioSource bgmAudioSource;

    Queue<AudioSource> sfxPool = new Queue<AudioSource>();


    private void Awake()
    {
        base.Awake();
        if(bgmAudioSource == null) bgmAudioSource = this.gameObject.AddComponent<AudioSource>();
    }

    public void BgmPlay(AudioClip _audioClip, float _volume = 0.5f)
    {
        bgmAudioSource.volume = _volume * OptionManager.Instance.SFXVolume;
        bgmAudioSource.clip = _audioClip;
        bgmAudioSource.Play();
    }

    public void SfxPlay(AudioClip _audioClip, float _volume = 0.5f)
    {
        AudioSource _audioSource;
        if (sfxPool.Count == 0) _audioSource = gameObject.AddComponent<AudioSource>();
        else _audioSource = sfxPool.Dequeue();
        _audioSource.volume = _volume * OptionManager.Instance.SFXVolume;
        _audioSource.clip = _audioClip;
        _audioSource.Play();
        StartCoroutine(WaitAudioEnd(_audioSource));
    }

    IEnumerator WaitAudioEnd(AudioSource audioSource)
    {
        if (audioSource.clip != null) yield return new WaitForSeconds(audioSource.clip.length);
        if (sfxPool != null) sfxPool.Enqueue(audioSource);
    }
}
