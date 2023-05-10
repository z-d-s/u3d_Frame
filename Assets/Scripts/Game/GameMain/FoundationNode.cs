using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoundationNode
{
    public GameObject nodeObj { get; set; }

    private FoundationNode preNode;
    public FoundationNode GetPreNode()
    {
        return this.preNode;
    }

    private FoundationNode nextNode;
    public FoundationNode GetNextNode()
    {
        return this.nextNode;
    }
    public void SetNextNode(FoundationNode node)
    {
        this.nextNode = node;
        this.nextNode.preNode = this;
    }

    /// <summary>
    /// 获取地基的位置
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPos()
    {
        return this.nodeObj.transform.position;
    }

    /// <summary>
    /// 设定地基的位置
    /// </summary>
    /// <param name="pos"></param>
    public void SetPos(Vector3 pos)
    {
        this.nodeObj.transform.position = pos;
    }

    /// <summary>
    /// 设定地基相对于前一个地基的偏移量位置()
    /// </summary>
    /// <param name="offsetPos"></param>
    public void SetOffsetPos(Vector3 offsetPos)
    {
        Vector3 prePos = Vector3.zero;
        if(preNode != null)
        {
            prePos = preNode.nodeObj.transform.position;
        }
        this.nodeObj.transform.position = prePos + offsetPos;
    }

    /// <summary>
    /// 回收
    /// </summary>
    public void Recycle()
    {
        PoolMgr.Instance.RecycleObject(this.nodeObj.name, this.nodeObj);
    }
}
