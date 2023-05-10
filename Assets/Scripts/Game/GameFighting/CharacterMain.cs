using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMain : CharacterBase
{
    /// <summary>
    /// 该角色当前所处地基节点
    /// </summary>
    public FoundationNode currentFoundationNode;

    private void Awake()
    {
        EventMgr.Instance.AddListener(EventDefine.MVC_UI_Game_Jump, this.Jump);
    }

    private void Update()
    {
        
    }

    private void OnDestroy()
    {
        EventMgr.Instance.RemoveListener(EventDefine.MVC_UI_Game_Jump, this.Jump);
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

        float angle = UtilTool.GetAngle_XZ(this.GetCharacterPos(), nextNode.GetPos());
        LogHelper.Log($"下一地块与角色自身 角度:{angle}");
    }
}
