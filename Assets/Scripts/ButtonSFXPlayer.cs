using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSFXPlayer : MonoBehaviour
{
    [SerializeField] AudioClip sfxClip;

    Button button;
    private void Awake()
    {
        button = this.GetComponent<Button>();
        if (button != null) button.onClick.AddListener(PlaySFX);
    }

    void PlaySFX()
    {
        if (sfxClip == null) return;
        SoundManager.Instance.SfxPlay(sfxClip);
    }
}
