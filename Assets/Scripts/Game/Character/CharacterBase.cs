/**
 * 角色基类
 */

using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    /// <summary>
    /// 移动指示箭头
    /// </summary>
    [HideInInspector]
    public CircleMarkCtrl circleMarkCtrl = null;

    /// <summary>
    /// 角色实体
    /// </summary>
    [HideInInspector]
    public GameObject real_Character;

    /// <summary>
    /// 角色动画控制脚本
    /// </summary>
    [HideInInspector]
    public AnimatorCtrl animCtrl = null;

    /// <summary>
    /// 是否死亡
    /// </summary>
    [HideInInspector]
    public bool isDeath;

    /// <summary>
    /// 队伍属性
    /// </summary>
    [HideInInspector]
    public Enum_TeamType teamType;

    /// <summary>
    /// 角色基础数据
    /// </summary>
    [HideInInspector]
    public CharacterBaseData characterBaseData = new CharacterBaseData();

    public void SetRealCharacter(GameObject real_Character)
    {
        this.real_Character = real_Character;
    }

    public void SetBaseControl()
    {
        this.AddCirlceMark();
        this.AddAnimatorCtrl();
    }
    
    private void AddCirlceMark()
    {
        if(this.circleMarkCtrl == null)
        {
            string markPath = "";
            if(this.teamType == Enum_TeamType.Self || this.teamType == Enum_TeamType.Team)
            {
                markPath = "Effects/Prefabs/Circle_Self.prefab";
            }
            else if(this.teamType == Enum_TeamType.Enemy)
            {
                markPath = "Effects/Prefabs/Circle_Enemy.prefab";
            }
            GameObject obj = PoolMgr.Instance.GetObject(UtilTool.GetNameFromPath(markPath));
            if(obj == null)
            {
                AssetsLoadMgr.Instance.LoadAsync("circle_mark", markPath, (string _assetName, UnityEngine.Object _obj) =>
                {
                    GameObject obj = PoolMgr.Instance.GetObject(_obj.name, _obj as GameObject);
                    this.circleMarkCtrl = obj.AddComponent<CircleMarkCtrl>();
                    this.circleMarkCtrl.SetTarget(this);
                });
            }
        }
        else
        {
            this.circleMarkCtrl.SetTarget(this);
        }
    }

    private void AddAnimatorCtrl()
    {
        if (this.animCtrl == null)
        {
            this.animCtrl = this.real_Character.AddComponent<AnimatorCtrl>();
            this.animCtrl.Init();
        }
        else
        {
            this.animCtrl.Init();
        }
    }

    public Vector3 GetCharacterPos()
    {
        return this.transform.position;
    }

    public void SetCharacterPos(Vector3 pos)
    {
        this.transform.position = pos;
    }

    public Vector3 GetCharacterEuler()
    {
        return this.real_Character.transform.localEulerAngles;
    }

    public Quaternion GetCharacterLocalRot()
    {
        return this.real_Character.transform.localRotation;
    }

    public Quaternion GetCharacterWorldRot()
    {
        return this.real_Character.transform.rotation;
    }

    public Vector3 GetCharacterForward()
    {
        return this.real_Character.transform.forward;
    }

    private void OnHurt(float attack)
    {
        if (this.isDeath == true)
        {
            return;
        }

        float lost = attack;
        if (lost <= 0)
        {
            return;
        }

        this.characterBaseData.hp -= lost;
        if (this.characterBaseData.hp <= 0)
        {
            this.isDeath = true;
        }
    }
}
