using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] Image medalImage;
    [SerializeField] Image lockImage;
    [SerializeField] AudioClip clickClip;
    [SerializeField] AudioClip lockClip;

    int level;
    bool isLock;

    private void Awake()
    {
        medalImage.enabled = false;
    }

    public void SetLevelText(int _level)
    {
        level = _level;
        levelText.text = _level.ToString();
    }

    public void Lock()
    {
        levelText.enabled = false;
        lockImage.enabled = true;
        isLock = true;
    }

    public void Unlock()
    {
        levelText.enabled = true;
        lockImage.enabled = false;
        isLock = false;
    }

    public void ShowMedal()
    {
        medalImage.enabled = true;
    }

    public void Btn_SelectLevel()
    {
        if(!isLock)
        {
            SceneLoader.Instance.LoadScene(level + 1);
            SoundManager.Instance.SfxPlay(clickClip);
        }
        else
        {
            SoundManager.Instance.SfxPlay(lockClip);
        }
    }
}
