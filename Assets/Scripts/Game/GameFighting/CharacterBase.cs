/**
 * 控制层组件
 */

using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    public bool isAlive;

    protected float hp;
    protected float define;

    public AnimatorCtrl animCtrl = null;
    public ShadowCtrl shadowCtrl = null;

    //正式项目使用Excel表格读取数据
    public void Init()
    {
        this.isAlive = true;
        this.hp = 1;
        this.define = 1;

        this.AddAnimator();
        this.AddShadow();
    }

    private void AddAnimator()
    {
        this.animCtrl = this.gameObject.AddComponent<AnimatorCtrl>();
        this.animCtrl.Init();
        this.animCtrl.SetState(AnimatorCtrl.AnimState.Idle);
    }

    private void AddShadow()
    {
        AssetsLoadMgr.Instance.LoadAsync("shadow", "Effects/Prefabs/Shadow.prefab", (string name, UnityEngine.Object obj) =>
        {
            GameObject _obj = PoolMgr.Instance.GetObject(obj.name, obj as GameObject);
            this.shadowCtrl = _obj.AddComponent<ShadowCtrl>();
            this.shadowCtrl.Init(this.transform);
        });
    }

    public Vector3 GetCharacterPos()
    {
        return this.transform.position;
    }

    private void OnEndBySkill()
    {
        if(this.isAlive == false)
        {
            return;
        }

        this.animCtrl.SetState(AnimatorCtrl.AnimState.Idle);
    }

    private void OnHurt(float attack)
    {
        if (this.isAlive == false)
        {
            return;
        }

        float lost = attack - define;
        if (lost <= 0)
        {
            return;
        }

        this.hp -= lost;
        if (this.hp <= 0)
        {
            this.isAlive = false;
            this.animCtrl.SetState(AnimatorCtrl.AnimState.Death);
        }
    }
}
