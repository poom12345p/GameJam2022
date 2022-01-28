using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
public class FloorTile : MapTile
{
    [ReadOnly]public BaseUnit UnitOnTile;
    
    public bool TryMoveTo(BaseUnit _unit)
    {
        if (UnitOnTile)
            return false;

        UnitOnTile = _unit;
            return true;

    }

    public void MoveOut(BaseUnit _unit)
    {
        if (UnitOnTile == _unit)
        {
            Debug.Log($"{_unit} is move out of {gameObject.name}");
            UnitOnTile = null;
        }
    }

    public bool TryGetUnit(out BaseUnit _unit)
    {
        _unit = UnitOnTile;
        return UnitOnTile != null;
    }
}
