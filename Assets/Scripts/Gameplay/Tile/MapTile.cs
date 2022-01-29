using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
public class MapTile : MonoBehaviour
{
    [ReadOnly] [SerializeField] protected Vector2Int  pos;
    public Vector2Int  Pos;
    protected MapManager mapManager;
    public void Init(MapManager _map)
    {
        mapManager = _map;
        Pos.x = Mathf.FloorToInt(transform.position.x);
        Pos.y = Mathf.FloorToInt(transform.position.y);
    }
}
