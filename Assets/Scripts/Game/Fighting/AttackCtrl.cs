using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackCtrl : MonoBehaviour
{
    private float hurtTime = 0f;
    private float endTime = 0f;

    private UnityAction hurtAction = null;
    private UnityAction endAction = null;

    private float nowTime = 0f;
    private bool isAttacking = false;

    public void Init()
    {
        this.hurtTime = 0f;
        this.endTime = 0f;
        this.hurtAction = null;
        this.endAction = null;
        this.nowTime = 0f;
        this.isAttacking = false;
    }

    public bool DoAttack(float hurtTime, float endTime, UnityAction HurtAction, UnityAction EndAction)
    {
        if(this.isAttacking)
        {
            return false;
        }

        this.hurtTime = hurtTime;
        this.endTime = endTime;
        this.hurtAction = HurtAction;
        this.endAction = EndAction;
        this.nowTime = 0f;
        this.isAttacking = true;

        return true;
    }

    private void Update()
    {
        if(this.isAttacking == false)
        {
            return;
        }

        nowTime += Time.deltaTime;
        if(nowTime >= hurtTime)
        {
            if(this.hurtAction != null)
            {
                this.hurtAction();
                this.hurtAction = null;
            }
        }

        if(nowTime >= endTime)
        {
            if(this.endAction != null)
            {
                this.endAction();
                this.endAction = null;
            }
            this.isAttacking = false;
        }
    }
}
