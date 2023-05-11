using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterMain : CharacterBase
{
    /// <summary>
    /// 该角色当前所处地基节点
    /// </summary>
    public FoundationNode currentFoundationNode;

    /// <summary>
    /// 跳跃那一刻角色位置
    /// </summary>
    private Vector3 oriJumpPos = Vector3.zero;
    /// <summary>
    /// 所需跳跃时间
    /// </summary>
    private float jumpTime = 0f;

    /// <summary>
    /// 速度X轴分量
    /// </summary>
    private float speed_X = 0f;
    /// <summary>
    /// 速度Z轴分量
    /// </summary>
    private float speed_Z = 0f;
    /// <summary>
    /// 当前跳跃中时间
    /// </summary>
    private float currentJumpTime = 0f;

    private void Awake()
    {
        EventMgr.Instance.AddListener(EventDefine.EVE_UI_Game_Jump, this.Jump);
    }

    private void Update()
    {
        if(GameMgr.Instance.gameState != GameState.Jumping)
        {
            return;
        }

        this.currentJumpTime += Time.deltaTime;
        if(this.currentJumpTime >= this.jumpTime)
        {
            LogHelper.Log("=== 一次跳跃结束 ===");
            GameMgr.Instance.gameState = GameState.Gameing;
            this.currentJumpTime = this.jumpTime;
        }
        float jumpPos_Y = Mathf.Sin(Mathf.PI * this.currentJumpTime / this.jumpTime) * 4;
        float jumpPos_X = this.speed_X * this.currentJumpTime;
        float jumpPos_Z = this.speed_Z * this.currentJumpTime;
        this.transform.position = new Vector3(this.oriJumpPos.x + jumpPos_X, jumpPos_Y, this.oriJumpPos.z + jumpPos_Z);
    }

    private void OnDestroy()
    {
        EventMgr.Instance.RemoveListener(EventDefine.EVE_UI_Game_Jump, this.Jump);
    }

    private void Jump(IEventArgs args)
    {
        if (args == null)
        {
            return;
        }
        LogHelper.Log($"开始跳跃,蓄力时间:{args.GetValue<float>()}");

        if(this.currentFoundationNode == null)
        {
            return;
        }

        FoundationNode nextNode = this.currentFoundationNode.GetNextNode();
        if(nextNode == null)
        {
            return;
        }

        this.currentJumpTime = 0f;
        this.oriJumpPos = this.GetCharacterPos();
        GameMgr.Instance.gameState = GameState.Jumping;

        float energyMoveTime = args.GetValue<float>();
        this.jumpTime = this.GetJumpTime(energyMoveTime);
        this.GetJumpDeviceSpeed(this.GetCharacterPos(), nextNode.GetPos(), energyMoveTime, out this.speed_X, out this.speed_Z);
    }

    /// <summary>
    /// 跳跃落点位置
    /// </summary>
    /// <param name="oriPos">起始点</param>
    /// <param name="targetPos">目标点</param>
    /// <param name="energyTime">蓄力时间</param>
    /// <returns></returns>
    private Vector3 GetJumpPointPos(Vector3 oriPos, Vector3 targetPos, float energyTime)
    {
        oriPos = new Vector3(oriPos.x, 0, oriPos.z);
        targetPos = new Vector3(targetPos.x, 0, targetPos.z);
        Vector3 offsetPos = targetPos - oriPos;
        float distance = Vector3.Distance(targetPos, oriPos);
        float speed_X = GameApp.Instance.config.energyMoveSpeed * offsetPos.x / distance;
        float speed_Z = GameApp.Instance.config.energyMoveSpeed * offsetPos.z / distance;

        Vector3 jumpPoint = new Vector3(oriPos.x + speed_X * energyTime, 0, oriPos.z + speed_Z * energyTime);
        return jumpPoint;
    }

    /// <summary>
    /// 获取跳跃时间
    /// </summary>
    /// <param name="energyTime"></param>
    /// <returns></returns>
    private float GetJumpTime(float energyTime)
    {
        return (GameApp.Instance.config.jumpMoveSpeed / GameApp.Instance.config.energyMoveSpeed) * energyTime;
    }

    /// <summary>
    /// 计算跳跃速度X、Z轴向分量
    /// </summary>
    /// <param name="oriPos">起始点</param>
    /// <param name="targetPos">目标点</param>
    /// <param name="energyTime">蓄力时间</param>
    /// <param name="x"></param>
    /// <param name="z"></param>
    private void GetJumpDeviceSpeed(Vector3 oriPos, Vector3 targetPos, float energyTime, out float x, out float z)
    {
        oriPos = new Vector3(oriPos.x, 0, oriPos.z);
        targetPos = new Vector3(targetPos.x, 0, targetPos.z);
        Vector3 offsetPos = targetPos - oriPos;
        float distance = Vector3.Distance(targetPos, oriPos);
        x = GameApp.Instance.config.energyMoveSpeed * offsetPos.x / distance;
        z = GameApp.Instance.config.energyMoveSpeed * offsetPos.z / distance;
    }
}
