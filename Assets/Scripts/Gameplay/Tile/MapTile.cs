using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
public class MapTile : MonoBehaviour
{
    [ReadOnly] [SerializeField] protected Vector2Int  pos;
    public bool ostacobstacle;

    public Vector2Int  Pos;
    public void Init()
    {
        Pos.x = Mathf.FloorToInt(transform.position.x);
        Pos.y = Mathf.FloorToInt(transform.position.y);
    }
}
