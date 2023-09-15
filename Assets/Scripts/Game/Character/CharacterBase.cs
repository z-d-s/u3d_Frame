/**
 * 控制层组件
 */

using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    public AnimatorCtrl animCtrl = null;
    public ShadowCtrl shadowCtrl = null;

    /// <summary>
    /// 是否死亡
    /// </summary>
    [HideInInspector]
    public bool isDeath;

    [HideInInspector]
    public BaseData characterData = new BaseData();

    //正式项目使用Excel表格读取数据
    public void Init()
    {
        this.isDeath = false;

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

    public void SetCharacterPos(Vector3 pos)
    {
        this.transform.position = pos;
    }

    private void OnEndBySkill()
    {
        if(this.isDeath == true)
        {
            return;
        }

        this.animCtrl.SetState(AnimatorCtrl.AnimState.Idle);
    }

    private void OnHurt(float attack)
    {
        if (this.isDeath == true)
        {
            return;
        }

        float lost = attack - this.characterData.defence;
        if (lost <= 0)
        {
            return;
        }

        this.characterData.hp -= lost;
        if (this.characterData.hp <= 0)
        {
            this.isDeath = true;
            this.animCtrl.SetState(AnimatorCtrl.AnimState.Death);
        }
    }
}
