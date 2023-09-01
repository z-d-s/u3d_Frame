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
        EventMgr.Instance.AddListener(EventDefine.EVE_Game_Jump, this.Jump);
        EventMgr.Instance.AddListener(EventDefine.EVE_GameRestart, this.GameRestart);
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
            this.JumpOver();
        }
        float jumpPos_Y = Mathf.Sin(Mathf.PI * this.currentJumpTime / this.jumpTime) * 4;
        float jumpPos_X = this.speed_X * this.currentJumpTime;
        float jumpPos_Z = this.speed_Z * this.currentJumpTime;
        this.transform.position = new Vector3(this.oriJumpPos.x + jumpPos_X, jumpPos_Y, this.oriJumpPos.z + jumpPos_Z);
    }

    private void OnDestroy()
    {
        EventMgr.Instance.RemoveListener(EventDefine.EVE_Game_Jump, this.Jump);
        EventMgr.Instance.RemoveListener(EventDefine.EVE_GameRestart, this.GameRestart);
    }

    /// <summary>
    /// 获取跳跃时间
    /// </summary>
    /// <param name="energyTime">蓄力时间</param>
    /// <returns></returns>
    private float GetJumpTime(float energyTime)
    {
        float jumpTime = (GameApp.Instance.config.energyMoveSpeed / GameApp.Instance.config.jumpMoveSpeed) * energyTime;
        return jumpTime;
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

        //计算跳跃速度X、Z轴向分量
        Vector3 oriPos = this.GetCharacterPos();
        oriPos = new Vector3(oriPos.x, 0, oriPos.z);
        Vector3 targetPos = nextNode.GetPos();
        targetPos = new Vector3(targetPos.x, 0, targetPos.z);
        Vector3 offsetPos = targetPos - oriPos;
        float distance = Vector3.Distance(targetPos, oriPos);
        this.speed_X = GameApp.Instance.config.jumpMoveSpeed * offsetPos.x / distance;
        this.speed_Z = GameApp.Instance.config.jumpMoveSpeed * offsetPos.z / distance;

        float energyMoveTime = args.GetValue<float>();
        this.jumpTime = this.GetJumpTime(energyMoveTime);
        LogHelper.LogRed($"jumpTime = {jumpTime}");
        this.currentJumpTime = 0f;
        this.oriJumpPos = this.GetCharacterPos();
        GameMgr.Instance.gameState = GameState.Jumping;
    }

    private void GameRestart(IEventArgs args)
    {
        if (GameMgr.Instance.firstFoundationNode != null)
        {
            //第一基座赋值为当前角色基座
            this.currentFoundationNode = GameMgr.Instance.firstFoundationNode;

            //角色位置复位
            this.transform.position = this.currentFoundationNode.GetPos();

            //目标参考球复位
            GameMgr.Instance.ball_Target.SetPos(this.transform.position);

            //标定参考球复位
            GameMgr.Instance.ball_Calibration.SetTargetNode(this.currentFoundationNode);
            GameMgr.Instance.ball_Calibration.SetTargetPos(this.currentFoundationNode.GetPos(), 0f);

            FoundationNode temp = this.currentFoundationNode.GetNextNode();
            while(temp != null)
            {
                temp.Recycle();
                temp = temp.GetNextNode();
            }
            this.currentFoundationNode.SetNextNode(null);
        }
    }

    private void JumpOver()
    {
        if (this.currentFoundationNode == null)
        {
            return;
        }

        bool bStillInThisFoundation = this.currentFoundationNode.CheckPosInFoundation(this.GetCharacterPos());
        if(bStillInThisFoundation)
        {
            LogHelper.LogRed("角色依然停留在当前基块上 === 游戏继续 ===");
        }
        else
        {
            FoundationNode nextNode = this.currentFoundationNode.GetNextNode();
            if(nextNode != null)
            {
                bool bInNextFoundation = nextNode.CheckPosInFoundation(this.GetCharacterPos());
                if(bInNextFoundation)
                {
                    LogHelper.LogRed("角色跳跃到了下一基块上 === 游戏继续 ===");
                    this.currentFoundationNode = nextNode;
                    GameMgr.Instance.ball_Calibration.SetTargetNode(this.currentFoundationNode);

                    FoundationNode temp_NextNode = this.currentFoundationNode.GetNextNode();
                    if (temp_NextNode == null)
                    {
                        LogHelper.LogBlue("需要新的基座 pro");
                        GameObject foundationObj = PoolMgr.Instance.GetObject("map_foundation", "Maps/Prefabs/Foundation_001.prefab");
                        FoundationNode node = new FoundationNode();
                        node.nodeObj = foundationObj;
                        this.currentFoundationNode.SetNextNode(node);
                        node.SetOffsetPos(new Vector3(Random.Range(-5f, 5f), 0, Random.Range(6f, 9f)));
                        node.Init(2f, 2f);
                    }
                    else
                    {
                        LogHelper.LogYellow("当前基座有后续基座，不需要新创建");
                        temp_NextNode.SetOffsetPos(new Vector3(Random.Range(-5f, 5f), 0, Random.Range(6f, 9f)));
                    }
                }
                else
                {
                    LogHelper.Log("角色不在当前基块上，也不再下一基块上 === 游戏结束 ===");
                    GameMgr.Instance.GameOver();
                }
            }
            else
            {
                LogHelper.Log("没找到下一个基块 === 游戏结束 ===");
                GameMgr.Instance.GameOver();
            }
        }
    }
}
