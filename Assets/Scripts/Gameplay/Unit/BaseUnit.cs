using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
[System.Serializable]
public class BaseUnit : MonoBehaviour
{
    public enum UnitType
    {
        RED,BLUE
    }

    public UnitType Type;

    MapManager mapManager;
    [ReadOnly] [SerializeField] FloorTile myTile;
    HashSet<BaseUnit> attachedUnit= new HashSet<BaseUnit>();
    protected bool isInit;
    public virtual void Init(MapManager _map)
    {
        mapManager = _map;
        isInit = true;
        if(mapManager.TryGetFloorTile(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y), out var _tile))
        {
            myTile = _tile;
        }
        else
           Debug.LogError($"{gameObject.name} didn't place on floor tile,pls relocate this unit");
    }
    #region move
    protected void MoveDirection(int _h,int _v)
    {
        if(mapManager.TryGetFloorTile(myTile.Pos.x + _h, myTile.Pos.y + _v,out var _tile)){
          if(TryMove(_tile)){
                InteractAdjacentTiles();
            }
        }
    }

    protected void MoveDirection(Vector2Int _dir)
    {
        if (mapManager.TryGetFloorTile(myTile.Pos.x + _dir.x, myTile.Pos.y + _dir.y, out var _tile))
        {
            if (TryMove(_tile))
            {
                InteractAdjacentTiles();
            }
        }
    }
    protected bool TryMove(FloorTile _floor)
    {
        if (_floor.TryMoveTo(this))
            MoveTo(_floor);

        return true;


    }

    protected void MoveTo(FloorTile mapTile)
    {
        myTile.MoveOut(this);
        myTile = mapTile;
        transform.position = mapTile.transform.position;
    }
    #endregion
    #region interaction

    public void InteractAdjacentTiles()
    {
        if (mapManager.TryGetFloorTile(new Vector2Int( myTile.Pos.x + 1, myTile.Pos.y), out var _tile1)){
            InteractTile(_tile1);
        }
        if (mapManager.TryGetFloorTile(new Vector2Int(myTile.Pos.x - 1, myTile.Pos.y), out var _tile2)){
            InteractTile(_tile2);
        }
        if (mapManager.TryGetFloorTile(new Vector2Int(myTile.Pos.x, myTile.Pos.y + 1), out var _tile3)){
            InteractTile(_tile3);
        }
        if (mapManager.TryGetFloorTile(new Vector2Int(myTile.Pos.x, myTile.Pos.y - 1), out var _tile4)){
            InteractTile(_tile4);
        }

    }

    protected void InteractTile(FloorTile _tile)
    {
        if(_tile.TryGetUnit(out var unit))
        {

        }
    }

    protected void InteractUnit(BaseUnit _unit)
    {
       if(_unit.Type != Type)
       {
            attachedUnit.Add(_unit);
            _unit.AddAttached(this);
       }
       else
        {
            var dir = _unit.GetFloor().Pos- myTile.Pos;
            MoveDirection(dir);
        }
    }
    #endregion

    public void AddAttached(BaseUnit _unit)
    {
        attachedUnit.Add(_unit);
    }

    public FloorTile GetFloor()
    {
        return myTile;
    }




}
