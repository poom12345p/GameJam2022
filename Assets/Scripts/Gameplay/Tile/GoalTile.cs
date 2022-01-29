using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GoalTile : FloorTile
{
    public override bool UnitMoveIn(BaseUnit _unit)
    {
        if (base.UnitMoveIn(_unit))
        {
            if (_unit is PlayerUnit)
            {
                mapManager.StageManager.CheckClearCondition((PlayerUnit)_unit);
            }
            return true;
        }
        return false;
    }
}
