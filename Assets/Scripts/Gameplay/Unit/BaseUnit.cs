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
    public Action<FloorTile> OnMove;
    public Action OnFinishedMove;
    public Action OnFinishedAllMoveState;
    public Action OnPush;
    public Action<BaseUnit>   OnBind;
    public Action<BaseUnit>  OnUnBind;
    MapManager mapManager;

    [ReadOnly, SerializeField] private FloorTile myTile;
    [SerializeField] private float moveCD = 0.2f;
    [SerializeField] AudioClip bindSound;
    [SerializeField] AudioClip unbindSound;
    [SerializeField] AudioClip bounceSound;
    List<BaseUnit> boundedUnits = new List<BaseUnit>();
    protected bool isInit;
    private BaseUnit coreMoveUnit;
    protected float moveCDCount=0.0f;
    bool isMoving=false;
    protected UnitSprite spriteBody;
    public List<BaseUnit> BoundedUnit { get => boundedUnits; }
    public float MoveCD { get => moveCD; }
    public FloorTile MyTile { get => myTile; }

    //protected bool isInterracted;

    public virtual void Init(MapManager _map)
    {
        mapManager = _map;
        isInit = true;
        if(mapManager.TryGetFloorTile(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y), out var _tile)){
            myTile = _tile;
            _tile.UnitMoveIn(this);
        }
        else
           Debug.LogError($"{gameObject.name} didn't place on floor tile,pls relocate this unit");
        spriteBody = GetComponentInChildren<UnitSprite>();
        if(spriteBody)
            spriteBody.Init(this);
        GetSound();
        StartBiundInteracting();
    }

    public void GetSound()
    {
        if(bindSound == null) bindSound= Resources.Load<AudioClip>("Audio/Snap_Pianolist_bip");
        if (unbindSound == null) unbindSound = Resources.Load<AudioClip>("Audio/break_Pianolist_bip");
        if (bounceSound == null) bounceSound = Resources.Load<AudioClip>("Audio/Bounce_Pianolist_bip");
    }


    protected virtual void Update()
    {
        if(moveCDCount > 0.0f)
        {
            moveCDCount -= Time.deltaTime;
        }
        if(isMoving && moveCDCount <= 0.0f)
        {
            isMoving = false;
            InteractAdjacentTiles(TryInteractPushTile);
        }
    }
    #region move

    public bool TryBePushedDirection(Vector2Int _dir, BaseUnit _unit)
    {
        if (moveCDCount > 0.0f) return false;
        SoundManager.Instance.SfxPlay(bounceSound);
        _unit.OnPush?.Invoke();
        OnPush?.Invoke();
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
        isMoving = false;
        if (TryMoveTo(_dir)) {
            isMoving = true;
            moveCDCount = moveCD;
            CustomFor(boundedUnits, _u => _u.TryMoveDirection(_dir, this));
        }
        else
            UnBindWith(_unit);
        if(coreMoveUnit == this) FinishedMove();
        return isMoving;
    }
    protected bool TryMoveTo(Vector2Int _dir)
    {
        if (mapManager.TryGetFloorTile(myTile.Pos.x + _dir.x, myTile.Pos.y + _dir.y, out var _floor)) {
            if (_floor.TryGetUnit(out var onUnit)){
                if (onUnit.TryMoveDirection(_dir, this)){
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
        if (mapManager.TryGetFloorTile(myTile.Pos.x + _velocity.x, myTile.Pos.y + _velocity.y, out var _floor)){
            if (_floor.TryGetUnit(out var onUnit)) {
                if (onUnit.CanMoveTo(_velocity)){
                    if (!boundedUnits.Contains(onUnit)) onUnit.TryMoveDirection(_velocity, onUnit);
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
        OnMove?.Invoke(mapTile);
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
        StartBiundInteracting();
        OnFinishedAllMoveState?.Invoke();
        OnFinishedAllMoveState = null;
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
    public void StartBiundInteracting()
    {
        int b = boundedUnits.Count;
        InteractAdjacentTiles(InteractBoundTile);
        if (b < boundedUnits.Count) SoundManager.Instance.SfxPlay(bindSound, 0.5f);
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
       if (boundedUnits.Contains(_unit))
            return;
       if(_unit.Type != Type){
            BindWith(_unit);
       }
    }
    public void BindWith(BaseUnit _unit)
    {
        Debug.Log($"{gameObject.name} bind {_unit.name}");
        AddAttached(_unit);
        _unit.AddAttached(this);
    }
    public void AddAttached(BaseUnit _unit)
    {

        if (!boundedUnits.Contains(_unit))
        {
            OnBind?.Invoke(_unit);
            boundedUnits.Add(_unit);
        }
    }
    public void UnBoundWithAll()
    {
        Debug.Log($"{gameObject.name}  unbind All");
        CustomFor(boundedUnits, (_unit) => {
            UnBindWith(_unit);
        });
    }
    public void UnBindWith(BaseUnit _unit)
    {
        Debug.Log($"{gameObject.name} unbound {_unit.name}");
        SoundManager.Instance.SfxPlay(unbindSound,0.2f);
        RemoveAttached(_unit);
        _unit.RemoveAttached(this);
    }
    public void RemoveAttached(BaseUnit _unit)
    {
        if (boundedUnits.Contains(_unit))
        {
            OnUnBind?.Invoke(_unit);
            boundedUnits.Remove(_unit);
        }
    }
    #endregion

    #region push
    protected void TryInteractPushTile(FloorTile _tile)
    {
       var _dir = Vector2Int.zero;
        if (_tile.TryGetUnit(out var unit))
        {
            TryInteractPushUnit(unit,out _dir);
        }
    }
    protected bool TryInteractPushUnit(BaseUnit _unit, out Vector2Int _dir)
    {
        _dir = Vector2Int.zero;
        if (boundedUnits.Contains(_unit))
            return false;
        if (_unit.Type == Type)
        {
            var dir = _unit.GetFloor().Pos - myTile.Pos;
            Debug.Log($"{gameObject.name} try push {_unit.name} with {dir}");
            if (_unit.CanMoveTo(dir))
            {
                Debug.Log($"{gameObject.name} push {_unit.name} with {dir}");
                _unit.TryBePushedDirection(dir, this);
            }
            else
            {
                Debug.Log($"{gameObject.name} is knocked back by{_unit.name} with {dir}");
                UnBoundWithAll();
               TryBePushedDirection(-dir, _unit);
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
