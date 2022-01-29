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
    public Action OnFinishedMove;
    MapManager mapManager;

    [ReadOnly,SerializeField] protected FloorTile myTile;
    [SerializeField] protected float moveCD = 0.2f;

    List<BaseUnit> boundedUnit= new List<BaseUnit>();
    List<BaseUnit> effectedUnit = new List<BaseUnit>();
    protected bool isInit;
    private BaseUnit coreMoveUnit;
    protected float moveCDCount=0.0f;

    //protected bool isInterracted;

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

        StartBoundInteracting();
    }


    protected virtual void Update()
    {
        if(moveCDCount > 0.0f)
        {
            moveCDCount -= Time.deltaTime;
        }
    }
    #region move

    public bool TryPushDirection(Vector2Int _dir, BaseUnit _unit)
    {
        if (moveCDCount > 0.0f) return false;
        Debug.Log($"{gameObject.name} pushed {_dir}");
        moveCDCount = moveCD;
        return TryMoveDirection(_dir, this);
    }
    public void TryMoveDirection(int _h,int _v,BaseUnit _unit)
    {
        TryMoveDirection(new Vector2Int(_h, _v), _unit);
    }

    public bool TryMoveDirection(Vector2Int _dir, BaseUnit _unit)
    {

        if (coreMoveUnit) return true;
        coreMoveUnit = _unit;
        if (coreMoveUnit != this) _unit.OnFinishedMove += FinishedMove;
        bool isMoved = false;
        if (TryMoveTo(_dir)) { 
            isMoved = true;
            moveCDCount = moveCD;
            CustomFor(effectedUnit, _u => _u.TryMoveDirection(_dir, this));
        }
        else
            UnBoundWith(_unit);
        if(coreMoveUnit == this) FinishedMove();
        return isMoved;
    }
    protected bool TryMoveTo(Vector2Int _dir)
    {
        if (mapManager.TryGetFloorTile(myTile.Pos.x + _dir.x, myTile.Pos.y + _dir.y, out var _floor))
        {
            if (_floor.TryGetUnit(out var onUnit))
            {
                Debug.Log($"{gameObject.name} movewith {_dir} but floor own by {onUnit}");
                if (onUnit.TryMoveDirection(_dir, this))
                {
                    return MoveTo(_floor); ;
                }

            }
            else
                return MoveTo(_floor); ;

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
                    if (!effectedUnit.Contains(onUnit)) onUnit.TryMoveDirection(_velocity, onUnit);
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
        Debug.Log($"{gameObject.name} move to {mapTile.Pos}");
        myTile.MoveOut(this);
        myTile = mapTile;
        myTile.UnitMoveIn(this);
        transform.position = mapTile.transform.position;
        return true;
    }

    public void FinishedMove()
    {
        OnFinishedMove?.Invoke();
        OnFinishedMove = null;
        coreMoveUnit = null;
        StartBoundInteracting();
        //InteractAdjacentTiles(TryInteractPushTile);
    }
    #endregion
    #region interaction
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
    #region bond
    public void StartBoundInteracting()
    {
        InteractAdjacentTiles(InteractBoundTile);
    }
    protected void InteractBoundTile(FloorTile _tile)
    {
        if(_tile.TryGetUnit(out var unit))
        {
            InteractBoundUnit(unit);
        }
    }

    protected void InteractBoundUnit(BaseUnit _unit)
    {
       if (effectedUnit.Contains(_unit))
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
        if (!effectedUnit.Contains(_unit))
            effectedUnit.Add(_unit);
    }
    public void UnBoundWithAll()
    {
        CustomFor(boundedUnit, (_unit) => {
            RemoveAttached(_unit);
            _unit.RemoveAttached(this);
        });
    }
    public void UnBoundWith(BaseUnit _unit)
    {
        Debug.Log($"{gameObject.name} unbound {_unit.name}");
        RemoveAttached(_unit);
        _unit.RemoveAttached(this);
        StartBoundInteracting();
    }
    public void RemoveAttached(BaseUnit _unit)
    {
        if (effectedUnit.Contains(_unit))
            effectedUnit.Remove(_unit);
    }
    #endregion

    #region push

    //protected bool TryGetPushedMove(out Vector2Int _dir)
    //{
    //    _dir = Vector2Int.zero;
    //    if (mapManager.TryGetFloorTile(new Vector2Int(myTile.Pos.x + 1, myTile.Pos.y), out var _tile1) && TryInteractPushTile(_tile1,out var _dir1))
    //    {
    //        _dir+=_dir1;
    //    }
    //    if (mapManager.TryGetFloorTile(new Vector2Int(myTile.Pos.x - 1, myTile.Pos.y), out var _tile2) && TryInteractPushTile(_tile2, out var _dir2))
    //    {
    //        _dir += _dir2;
    //    }
    //    if (mapManager.TryGetFloorTile(new Vector2Int(myTile.Pos.x, myTile.Pos.y + 1), out var _tile3) && TryInteractPushTile(_tile3, out var _dir3))
    //    {
    //        _dir += _dir3;
    //    }
    //    if (mapManager.TryGetFloorTile(new Vector2Int(myTile.Pos.x, myTile.Pos.y - 1), out var _tile4) && TryInteractPushTile(_tile4, out var _dir4))
    //    {
    //        _dir += _dir4;
    //    }
    //    return !_dir.Equals(Vector2Int.zero);
    //}

    
    protected void TryInteractPushTile(FloorTile _tile)
    {
       var _dir = Vector2Int.zero;
        if (_tile.TryGetUnit(out var unit))
        {
            TryInteractPushUnit(unit,out _dir);
        }
        //return false;
    }
    protected bool TryInteractPushUnit(BaseUnit _unit, out Vector2Int _dir)
    {
        _dir = Vector2Int.zero;
        if (effectedUnit.Contains(_unit))
            return false;
        if (_unit.Type == Type)
        {
            var dir = _unit.GetFloor().Pos - myTile.Pos;
            Debug.Log($"{gameObject.name} try push {_unit.name} with {dir}");
            if (_unit.CanMoveTo(dir))
            {
                Debug.Log($"{gameObject.name} push {_unit.name} with {dir}");
                _unit.TryPushDirection(dir, _unit);
            }
            else
            {
                Debug.Log($"{gameObject.name} is knocked back by{_unit.name} with {dir}");
                UnBoundWithAll();
                TryPushDirection(-dir, this);
                _dir = -dir;
                return true;
            }
        }
        return false;
    }

    protected void ResetMoveCD()
    {
        moveCDCount = 0.0f;
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
