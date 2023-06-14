using DG.Tweening;
using UnityEngine;

public class ReferenceBall_Calibration : MonoBehaviour
{
    private FoundationNode curNode = null;
    private void Awake()
    {
        EventMgr.Instance.AddListener(EventDefine.EVE_Game_SetNextNode, this.SetNextNode);
        this.gameObject.transform.localScale = Vector3.one * 0.3f;
    }

    private void OnDestroy()
    {
        EventMgr.Instance.RemoveListener(EventDefine.EVE_Game_SetNextNode, this.SetNextNode);
    }

    private void SetNextNode(IEventArgs args)
    {
        if (args == null)
        {
            return;
        }
        FoundationNode[] first_end_node = args.GetValue<FoundationNode[]>();
        if(first_end_node.Length != 2)
        {
            return;
        }
        LogHelper.Log($"curNode.InstanceID: {this.curNode.nodeObj.GetInstanceID()} | first_end_node[0].InstanceID: {first_end_node[0].nodeObj.GetInstanceID()}");
        if(this.curNode.nodeObj.GetInstanceID() != first_end_node[0].nodeObj.GetInstanceID())
        {
            return;
        }

        int count = first_end_node.Length;
        float sum_X = 0f, sum_Z = 0f;
        for(int i = 0; i < count; ++i)
        {
            sum_X += first_end_node[i].GetPos().x;
            sum_Z += first_end_node[i].GetPos().z;
        }
        Vector3 midPos = new Vector3(sum_X / count, 0f, sum_Z / count);
        this.SetTargetPos(midPos);
    }

    public void SetTargetNode(FoundationNode node)
    {
        this.curNode = node;
    }

    public void SetTargetPos(Vector3 targetPos, float duration = 0.5f)
    {
        if(duration == 0)
        {
            this.gameObject.transform.position = targetPos;
        }
        else
        {
            this.gameObject.transform.DOMove(targetPos, duration);
        }
    }
}