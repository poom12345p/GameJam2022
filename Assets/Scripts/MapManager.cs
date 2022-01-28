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
            AddTilessToDict(mapTiles.GetChild(i));
        }
    }

    public void AddTilessToDict(Transform tile)
    {
        int _x = (int)tile.position.x;
        int _y = (int)tile.position.y;
        if (!tilesData.ContainsKey(_x))
            tilesData.Add(_x, new Dictionary<int, MapTile>());

        if (!tilesData[_x].ContainsKey(_y))
            tilesData[_x].Add(_y, tile.gameObject.GetComponent<MapTile>());
        else
            Debug.LogError($"maptile are duplicate at {_x} {_y}");
    }
}
