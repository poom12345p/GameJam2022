using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class UnitSprite : MonoBehaviour
{
    BaseUnit unit;
    Tween moveTween;
    [SerializeField] LinkParticleControl linkParticleControl;
    public void Init(BaseUnit _unit)
    {
        unit = _unit;
        transform.SetParent(null);
        unit.OnMove += PrefromMove;
        linkParticleControl.Init(_unit);
    }

    public void PrefromMove(FloorTile _tile)
    {
        Debug.Log("Prefrommove");
        moveTween=transform.DOMove(_tile.transform.position, unit.MoveCD,false).SetEase(Ease.OutQuart);
    }

}
