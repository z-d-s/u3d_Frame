using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterMain : CharacterBase
{
    /// <summary>
    /// 移动过速度
    /// </summary>
    [HideInInspector]
    public float moveSpeed = 0.02f;

    /// <summary>
    /// 摇杆输入向量
    /// </summary>
    private Vector3 handleInput = Vector3.zero;

    private void Awake()
    {
        EventMgr.Instance.AddListener(EventDefine.EVE_GameControl_Move, this.PlayerMove);
        EventMgr.Instance.AddListener(EventDefine.EVE_GameRestart, this.GameRestart);
    }

    private void Update()
    {
        if (this.handleInput == Vector3.zero)
        {
            return;
        }

        this.transform.Translate(this.handleInput * this.moveSpeed, Space.World);
        this.transform.localRotation = Quaternion.Euler(new Vector3(0f, Vector3.SignedAngle(Vector3.forward, this.handleInput, Vector3.up), 0f));
    }

    private void OnDestroy()
    {
        EventMgr.Instance.RemoveListener(EventDefine.EVE_GameControl_Move, this.PlayerMove);
        EventMgr.Instance.RemoveListener(EventDefine.EVE_GameRestart, this.GameRestart);
    }

    private void PlayerMove(IEventArgs args)
    {
        if (args == null)
        {
            return;
        }

        Vector2 temp = args.GetValue<Vector2>();
        this.handleInput = new Vector3(temp.x, 0f, temp.y);
    }

    private void GameRestart(IEventArgs args)
    {
        
    }
}
