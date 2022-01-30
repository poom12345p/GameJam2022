using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelButton : MonoBehaviour
{
    public string SceneName { get => sceneName; set => sceneName = value; }

    [SerializeField] Image backgroundImage;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] Image stampImage;
    [SerializeField] Image trophyImage;
    [SerializeField] Image lockImage;
    [SerializeField] AudioClip clickClip;
    [SerializeField] AudioClip lockClip;
    [SerializeField] List<Sprite> spritesList;

    string sceneName;
    bool isLock;

    private void Awake()
    {
        trophyImage.enabled = false;
        stampImage.enabled = false;
        backgroundImage.sprite = spritesList[Random.Range(0, spritesList.Count)];
    }

    public void SetLevelText(int _level)
    {
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
        trophyImage.enabled = true;
    }

    public void ShowStamp()
    {
        stampImage.enabled = true;
    }

    public void Btn_SelectLevel()
    {
        if(!isLock)
        {
            SceneLoader.Instance.LoadScene(sceneName);
            SoundManager.Instance.SfxPlay(clickClip);
        }
        else
        {
            SoundManager.Instance.SfxPlay(lockClip);
        }
    }
}
