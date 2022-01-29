using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Unity.Collections;
public class StageManager : MonoBehaviour
{
    [SerializeField] InGameUI inGameUI;
    [SerializeField] PauseUI pauseUI;
    [SerializeField] int minimumMoves;
    [SerializeField] RewardUI reward;
    MapManager mapManager;
    List<BaseUnit> baseUnits=new List<BaseUnit>();
    [ReadOnly][SerializeField]BaseUnit objective;
    int moves = 0;
    bool isClear=false;

    private void OnValidate()
    {
        mapManager = gameObject.GetComponent<MapManager>() == null ? gameObject.AddComponent<MapManager>() : gameObject.GetComponent<MapManager>();
    }
    private void Start()
    {
        mapManager.Init(this);

        var unitTiles = GameObject.FindGameObjectWithTag("UnitTiles").transform;
        for (int i = 0; i < unitTiles.childCount; i++)
        {
            var _unit = unitTiles.GetChild(i).gameObject.GetComponent<BaseUnit>();
            _unit.Init(mapManager);
            FiltterUnit(_unit);
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

    protected void FiltterUnit(BaseUnit _unit)
    {
        if (_unit is PlayerUnit) SubscribePlayer((PlayerUnit)_unit);
        else if (_unit.gameObject.GetComponent<Objective>() != null) objective = _unit;
    }
    private void SubscribePlayer(PlayerUnit _unit)
    {
        _unit.OnMove += (_dir) =>
        {
            moves++;
            inGameUI.SetMove(moves);
        };
    }

    public void CheckClearCondition(PlayerUnit _player)
    {
        Debug.Log("Cheack Clear");
        if (objective)
        {
            if (_player.BoundedUnit.Contains(objective)) 
                Clear(); 
        }
        else
        {
            Clear();
        }
    }

    protected void Clear()
    {
        if (isClear) return;
        Debug.Log("Win Clear");
        reward.Show();
        isClear = true;
    }
}
