/**
 * 控制层组件
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorCtrl : MonoBehaviour
{
    public bool isAlive;

    protected float hp;
    protected float define;

    protected float skillAttack;
    protected float skillAttackRadius;
    protected float skillHurtTime;
    protected float skillEndTime;

    protected AnimCtrl animCtrl = null;
    protected AttackCtrl attackCtrol = null;

    public virtual void OnHurt(float attack)
    {
        if(this.isAlive == false)
        {
            return;
        }

        float lost = attack - define;
        if(lost <= 0)
        {
            return;
        }

        this.hp -= lost;
        if(this.hp <= 0)
        {
            this.isAlive = false;
            this.animCtrl.SetState(AnimCtrl.AnimState.Death);
        }
    }

    //正式项目使用Excel表格读取数据
    public void Init(/*params*/)
    {
        this.isAlive = true;
        this.hp = 5;
        this.define = 1;

        this.skillAttack = 2.0f;
        this.skillAttackRadius = 10.0f;
        this.skillHurtTime = 1.0f;
        this.skillEndTime = 1.4f;

        this.animCtrl = this.gameObject.AddComponent<AnimCtrl>();
        this.animCtrl.Init();
        this.animCtrl.SetState(AnimCtrl.AnimState.Idle);

        this.attackCtrol = this.gameObject.AddComponent<AttackCtrl>();
        this.attackCtrol.Init();
    }

    public void OnPlayerSkill()
    {
        if(this.attackCtrol.DoAttack(this.skillHurtTime, this.skillEndTime, this.OnComputeHurtBySkill, this.OnEndBySkill))
        {
            this.animCtrl.SetState(AnimCtrl.AnimState.Skill);
        }
    }

    private void OnComputeHurtBySkill()
    {
        List<GameObject> objs = GameMgr.Instance.findCharactorInRadius(this.transform.position, this.skillAttackRadius);
        for(int i = 0; i < objs.Count; ++i)
        {
            CharactorCtrl ctrl = objs[i].GetComponent<CharactorCtrl>();
            ctrl.OnHurt(this.skillAttack);
        }
    }

    private void OnEndBySkill()
    {
        if(this.isAlive == false)
        {
            return;
        }

        this.animCtrl.SetState(AnimCtrl.AnimState.Idle);
    }
}
