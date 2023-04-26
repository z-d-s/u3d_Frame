using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    /// <summary>
    /// 跟随的目标物体
    /// </summary>
    private GameObject target = null;

    /// <summary>
    /// 相对跟随物体的位置偏移
    /// </summary>
    private Vector3 offsetPos = Vector3.zero;

    /// <summary>
    /// 跟随系数 值越小 跟随约平滑[0 -- ratio -- 1]
    /// </summary>
    private float followRatio = 1f;

    public void SetTarget(GameObject _target, Vector3 _offsetPos = new Vector3(), float _followRatio = 1f)
    {
        this.target = _target;
        this.offsetPos = _offsetPos;
        this.followRatio = _followRatio;
    }

    public void SetOffsetPos(Vector3 _offsetPos = new Vector3())
    {
        this.offsetPos = _offsetPos;
    }

    public void SetFollowRatio(float _followRatio = 1f)
    {
        this.followRatio = _followRatio;
    }

    private void Update()
    {
        if(this.target)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, this.target.transform.position + this.offsetPos, this.followRatio);
        }
    }
}
