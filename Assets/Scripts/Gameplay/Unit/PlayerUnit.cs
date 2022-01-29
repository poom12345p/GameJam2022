using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerUnit : BaseUnit
{
    [SerializeField]private float moveCD =0.25f;
    private float moveCDCount=0.0f;
    int _vh=0, _vv = 0;
    private void Update()
    {
        if (!isInit) return;
        if (moveCDCount > 0.0f)
            moveCDCount -= Time.deltaTime;
        else{
            _vh = IsButtonRight() ? 1 : IsButtonLeft() ? -1 : 0;
            _vv = IsButtonUp() ? 1: IsButtonDown()?-1:0;
            if (_vv != 0 || _vh != 0){
                moveCDCount = moveCD;
                TryMoveDirection(_vh, _vv,this);
            }
        }
    }

    bool IsButtonUp()
    {
        return Input.GetButton("Vertical") && Input.GetAxis("Vertical") > 0.0f;
    }

    bool IsButtonDown()
    {
        return Input.GetButton("Vertical") && Input.GetAxis("Vertical") < 0.0f;
    }

    bool IsButtonLeft()
    {
        return Input.GetButton("Horizontal") && Input.GetAxis("Horizontal") < 0.0f;
    }

    bool IsButtonRight()
    {
        return Input.GetButton("Horizontal") && Input.GetAxis("Horizontal") > 0.0f;
    }


}
