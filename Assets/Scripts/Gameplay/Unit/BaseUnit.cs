using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using System;

[System.Serializable]
public class BaseUnit : MonoBehaviour
{
    public enum UnitType
    {
        RED,BLUE
    }

    public UnitType Type;
    public Action<Vector2Int> OnMove;
    MapManager mapManager;
    [ReadOnly] [SerializeField] FloorTile myTile;
    List<BaseUnit> attachedUnit= new List<BaseUnit>();
    protected bool isInit;
    private BaseUnit coreMoveUnit;
    protected bool isInterracted;
    public virtual void Init(MapManager _map)
    {
        mapManager = _map;
        isInit = true;
        if(mapManager.TryGetFloorTile(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y), out var _tile))
        {
            myTile = _tile;
            _tile.UnitMoveIn(this);
        }
        else
           Debug.LogError($"{gameObject.name} didn't place on floor tile,pls relocate this unit");
    }
    #region move
    public void TryMoveDirection(int _h,int _v,BaseUnit _unit)
    {
        TryMoveDirection(new Vector2Int(_h, _v), _unit);
    }

    public bool TryMoveDirection(Vector2Int _velocity, BaseUnit _unit)
    {
        if (coreMoveUnit) return true;
        Debug.Log($"{gameObject.name} try move to {_velocity}");
        coreMoveUnit = _unit;
        bool isMoved = false;
        if (TryMove(_velocity))
        {
            CustomFor(attachedUnit, _u => _u.TryMoveDirection(_velocity, this));
            isMoved = true;
        }
        else
            UnBoundWith(_unit);
        if (coreMoveUnit == this)
            StartInteracting();
        return isMoved;
    }
    protected bool TryMove( Vector2Int _velocity)
    {
        if (mapManager.TryGetFloorTile(myTile.Pos.x + _velocity.x, myTile.Pos.y + _velocity.y, out var _floor)){
            if (_floor.TryGetUnit(out var onUnit))
            {
                if (onUnit.TryMoveDirection(_velocity, this))
                {
                    return MoveTo(_floor);
                }
                else
                   return false;
            }
            else
                return MoveTo(_floor);
        }
        return false;

    }
    public bool CanMoveTo(Vector2Int _velocity)
    {
        if (mapManager.TryGetFloorTile(myTile.Pos.x + _velocity.x, myTile.Pos.y + _velocity.y, out var _floor))
        {
            if (_floor.TryGetUnit(out var onUnit))
            {
                if (onUnit.CanMoveTo(_velocity))
                {
                    Debug.Log($"{gameObject.name} found {onUnit.name}");
                    if (!attachedUnit.Contains(onUnit)) onUnit.TryMoveDirection(_velocity, onUnit);
                    return true;
                }
            }
            else
                return true;
        }
        return false;
    }
    protected bool MoveTo(FloorTile mapTile)
    {
        myTile.MoveOut(this);
        myTile = mapTile;
        myTile.UnitMoveIn(this);
        transform.position = mapTile.transform.position;
        return true;
    }

    #endregion
    #region interaction

    public void StartInteracting()
    {
        if (isInterracted) return;
        isInterracted = true;
        InteractAdjacentTiles(InteractBoundTile);
        attachedUnit.ForEach(_u => _u.StartInteracting());
        coreMoveUnit = null;
        isInterracted = false;
        InteractAdjacentTiles(InteractPushTile);
    }
    public void InteractAdjacentTiles(Action<FloorTile> _interMethod)
    {
        if (mapManager.TryGetFloorTile(new Vector2Int( myTile.Pos.x + 1, myTile.Pos.y), out var _tile1)){
            _interMethod?.Invoke(_tile1);
        }
        if (mapManager.TryGetFloorTile(new Vector2Int(myTile.Pos.x - 1, myTile.Pos.y), out var _tile2)){
            _interMethod?.Invoke(_tile2);
        }
        if (mapManager.TryGetFloorTile(new Vector2Int(myTile.Pos.x, myTile.Pos.y + 1), out var _tile3)){
            _interMethod?.Invoke(_tile3);
        }
        if (mapManager.TryGetFloorTile(new Vector2Int(myTile.Pos.x, myTile.Pos.y - 1), out var _tile4)){
            _interMethod?.Invoke(_tile4);
        }
    }
    #region hold
    protected void InteractBoundTile(FloorTile _tile)
    {
        if(_tile.TryGetUnit(out var unit))
        {
            InteractBoundUnit(unit);
        }
    }

    protected void InteractBoundUnit(BaseUnit _unit)
    {
       if (attachedUnit.Contains(_unit))
            return;
       if(_unit.Type != Type){
            Debug.Log($"{gameObject.name} hold {_unit.name}");
            BoundWith(_unit);
       }
    }
    public void BoundWith(BaseUnit _unit)
    {
        AddAttached(_unit);
        _unit.AddAttached(this);
    }
    public void AddAttached(BaseUnit _unit)
    {
        if (!attachedUnit.Contains(_unit))
            attachedUnit.Add(_unit);
    }

    public void UnBoundWith(BaseUnit _unit)
    {
        Debug.Log($"{gameObject.name} unbound {_unit.name}");
        RemoveAttached(_unit);
        _unit.RemoveAttached(this);
        StartInteracting();
    }
    public void RemoveAttached(BaseUnit _unit)
    {
        if (attachedUnit.Contains(_unit))
            attachedUnit.Remove(_unit);
    }
    #endregion
    #region push
    protected void InteractPushTile(FloorTile _tile)
    {
        if (_tile.TryGetUnit(out var unit))
        {
            InteractPushUnit(unit);
        }
    }
    protected void InteractPushUnit(BaseUnit _unit)
    {
        if (attachedUnit.Contains(_unit))
            return;
        if (_unit.Type == Type)
        {
            var dir = _unit.GetFloor().Pos - myTile.Pos;
            Debug.Log($"{gameObject.name} try push {_unit.name} with {dir}");
            if (_unit.CanMoveTo(dir))
            {
                Debug.Log($"{gameObject.name} push {_unit.name} with {dir}");
                _unit.TryMoveDirection(dir, _unit);
            }
            else
            {
                Debug.Log($"{gameObject.name} is knocked back by{_unit.name} with {dir}");
                TryMoveDirection(-dir, this);
            }
        }
    }
    #endregion
    #endregion
  

    public FloorTile GetFloor()
    {
        return myTile;
    }

    public void CustomFor<T>(List<T> _list,Action<T> _action)
    {
        int i = 0;
        while(i < _list.Count)
        {
            var count = _list.Count;
            _action?.Invoke(_list[i]);
            if (_list.Count < count) i--;
            i++;

        }
    }



}
