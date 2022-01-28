using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(StageManager))]
public class MapManager : MonoBehaviour
{
    
    [SerializeField]Transform mapTiles;
    Dictionary<int, Dictionary<int, MapTile>> tilesData;

    public void Init()
    {
        mapTiles = GameObject.FindGameObjectWithTag("MapTiles").transform;
        tilesData = new Dictionary<int, Dictionary<int, MapTile>>();
        for (int i = 0; i < mapTiles.childCount; i++){
            AddTilessToDict(mapTiles.GetChild(i).GetComponent<MapTile>());
        }
    }

    public void AddTilessToDict(MapTile tile)
    {
        if(!tile)Debug.Log($"tile null");
        tile.Init();
        if (!tilesData.ContainsKey(tile.Pos.x))
            tilesData.Add(tile.Pos.x, new Dictionary<int, MapTile>());

        if (!tilesData[tile.Pos.x].ContainsKey(tile.Pos.y))
            tilesData[tile.Pos.x].Add(tile.Pos.y, tile.gameObject.GetComponent<MapTile>());
        else
            Debug.LogError($"maptile are duplicate at {tile.Pos.x} {tile.Pos.y}");
    }

    public bool TryGetMapTile(Vector2Int _pos,out MapTile _tile)
    {
        _tile = null;
        if (!tilesData.ContainsKey(_pos.x))
            return false;
        if(!tilesData[_pos.x].ContainsKey(_pos.y))
            return false;
        _tile = tilesData[_pos.x][_pos.y];
        return true;
    }

    public bool TryGetFloorTile(Vector2Int pos, out FloorTile _floorTile)
    {
        _floorTile = null;
        if (TryGetMapTile(pos, out var _tile) && (_tile is FloorTile))
        {
            _floorTile = (FloorTile)_tile;
            return true;
        }
        return false;

    }

    public bool TryGetFloorTile(int x, int y, out FloorTile _floorTile)
    {
        return TryGetFloorTile(new Vector2Int(x, y), out _floorTile);

    }
}
