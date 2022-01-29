using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StageManager : MonoBehaviour
{
    [SerializeField] InGameUI inGameUI;
    [SerializeField] PauseUI pauseUI;

    MapManager mapManager;
    List<BaseUnit> baseUnits=new List<BaseUnit>();

    private void OnValidate()
    {
        mapManager = gameObject.GetComponent<MapManager>() == null ? gameObject.AddComponent<MapManager>() : gameObject.GetComponent<MapManager>();
    }
    private void Start()
    {
        mapManager.Init();

        var unitTiles = GameObject.FindGameObjectWithTag("UnitTiles").transform;
        for (int i = 0; i < unitTiles.childCount; i++)
        {
            var _unit = unitTiles.GetChild(i).gameObject.GetComponent<BaseUnit>();
            _unit.Init(mapManager);
            baseUnits.Add(_unit);
        }
        unitTiles.GetComponent<TilemapRenderer>().enabled = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            pauseUI.Show();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneLoader.Instance.ReloadScene();
        }
    }
}
