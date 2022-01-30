using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectUI : BaseUIAnimator
{
    [Header("LevelSelect Settings")]
    [SerializeField] LevelButton buttonPrefab;
    [SerializeField] List<string> scenesList;

    private void Awake()
    {
        for (int i = 0; i < scenesList.Count; i++)
        {
            CreateLevelButton(i);
        }
    }

    public LevelButton CreateLevelButton(int _index)
    {
        var _button = Instantiate(buttonPrefab, buttonPrefab.transform.parent);
        _button.SetLevelText(_index + 1);
        _button.SceneName = scenesList[_index];
        _button.gameObject.SetActive(true);
        UpdateButtonStatus(_button, _index);
        return _button;
    }

    void UpdateButtonStatus(LevelButton _button, int _index)
    {
        if (GameManager.Instance.GetInt(scenesList[_index] + "_Trophy") == 1) _button.ShowMedal();
        if (_button.transform.GetSiblingIndex() <= GameManager.Instance.UnlockedLevel)
        {
            if (_button.transform.GetSiblingIndex() != GameManager.Instance.UnlockedLevel) _button.ShowStamp();
            _button.Unlock();
        }
        else _button.Lock();
    }

    public void Back_Btn()
    {
        SceneLoader.Instance.LoadMenuScene();
    }
}
