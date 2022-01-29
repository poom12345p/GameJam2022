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
            var _button = Instantiate(buttonPrefab, buttonPrefab.transform.parent);
            _button.SetLevelText(i + 1);
            _button.SceneName = scenesList[i];
            _button.gameObject.SetActive(true);
            if (_button.transform.GetSiblingIndex() <= GameManager.Instance.UnlockedLevel) _button.Unlock();
            else _button.Lock();
        }
    }

    public void Back_Btn()
    {
        SceneLoader.Instance.LoadMenuScene();
    }
}
