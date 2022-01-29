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
                var player = (PlayerUnit)_unit;
                player.OnFinishedMove += ()=>mapManager.StageManager.CheckClearCondition(player);
            }
            return true;
        }
        return false;
    }
}
