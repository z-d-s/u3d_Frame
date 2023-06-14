using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceBall_Target : MonoBehaviour
{
    private void Awake()
    {
        EventMgr.Instance.AddListener(EventDefine.EVE_Game_RefreshTargetPos, this.RefreshTargetPos);
        this.gameObject.transform.localScale = Vector3.one * 0.3f;
    }

    private void OnDestroy()
    {
        EventMgr.Instance.RemoveListener(EventDefine.EVE_Game_RefreshTargetPos, this.RefreshTargetPos);
    }

    private void RefreshTargetPos(IEventArgs args)
    {
        if (args == null)
        {
            return;
        }

        if(GameMgr.Instance.characterMain.currentFoundationNode == null)
        {
            return;
        }

        FoundationNode nextNode = GameMgr.Instance.characterMain.currentFoundationNode.GetNextNode();
        if (nextNode == null)
        {
            return;
        }

        //计算跳跃速度X、Z轴向分量
        Vector3 oriPos = GameMgr.Instance.characterMain.GetCharacterPos();
        oriPos = new Vector3(oriPos.x, 0, oriPos.z);
        Vector3 targetPos = nextNode.GetPos();
        targetPos = new Vector3(targetPos.x, 0, targetPos.z);
        Vector3 offsetPos = targetPos - oriPos;
        float distance = Vector3.Distance(targetPos, oriPos);
        float speed_X = GameApp.Instance.config.energyMoveSpeed * offsetPos.x / distance;
        float speed_Z = GameApp.Instance.config.energyMoveSpeed * offsetPos.z / distance;

        float touchingTime = args.GetValue<float>();
        this.transform.position = new Vector3(oriPos.x + touchingTime * speed_X, 0, oriPos.z + touchingTime * speed_Z);
    }

    public void SetPos(Vector3 pos)
    {
        this.transform.position = pos;
    }
}