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
    int moves = 0;

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
            if (_unit is PlayerUnit) SubscribePlayer((PlayerUnit)_unit);
            baseUnits.Add(_unit);
        }
        unitTiles.GetComponent<TilemapRenderer>().enabled = false;
        inGameUI.SetMove(moves);
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

    private void SubscribePlayer(PlayerUnit _unit)
    {
        _unit.OnMove += (_dir) =>
        {
            moves++;
            inGameUI.SetMove(moves);
        };
    }
}
