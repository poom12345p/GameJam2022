using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerUnit : BaseUnit
{
    [SerializeField]private float inputCD =0.25f;
    [SerializeField] AudioClip moveSound;
    private float inputCDCount=0.0f;
    int _vh=0, _vv = 0;
    public bool canMove = true;

    public override void Init(MapManager _map)
    {
        base.Init(_map);
        moveSound= Resources.Load<AudioClip>("Audio/Walk_bip");
    }
    protected override void Update()
    {
        base.Update();
        if (!isInit) return;
        if (inputCDCount > 0.0f)
            inputCDCount -= Time.deltaTime;
        if(canMove && moveCDCount <= 0.0f && inputCDCount <= 0.0f){
            ResetInput();
            _vh = IsButtonRight() ? 1 : IsButtonLeft() ? -1 : 0;
            if(_vh == 0) _vv = IsButtonUp() ? 1: IsButtonDown()?-1:0;
            if (_vv != 0 || _vh != 0){
                inputCDCount = inputCD;
                SoundManager.Instance.SfxPlay(moveSound,0.2f);
                TryMoveDirection(_vh, _vv,this);
            }
        }
    }
    public void ResetInput()
    {
        _vh = 0;
        _vv = 0;
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
