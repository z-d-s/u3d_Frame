using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Gun : MonoBehaviour
{
    /// <summary>
    /// 武器持有者
    /// </summary>
    public CharacterBase owner;

    public Weapon_GunData weaponData = new Weapon_GunData();

    public void Init(CharacterBase owner, Transform hangPoint, int level = 1)
    {
        this.owner = owner;
        this.SetHangPoint(hangPoint);
        this.RefreshByLevel(level);
    }

    private void SetHangPoint(Transform hangPoint)
    {
        this.transform.parent = hangPoint;
        this.transform.localPosition = Vector3.zero;
        this.transform.localRotation = Quaternion.identity;
        this.transform.localScale = Vector3.one;
    }

    private void RefreshByLevel(int level)
    {
        Tbl_Weapon_Level tbl_Character_Level = TableMgr.Instance.GetTable<Tbl_Weapon_Level>(UtilTool.GetIDByIDAndLevel(this.weaponData.id, level));
        if (tbl_Character_Level == null)
        {
            tbl_Character_Level = TableMgr.Instance.GetTable<Tbl_Weapon_Level>(UtilTool.GetIDByIDAndLevel(this.weaponData.id, 1));
        }

        this.weaponData.attackSpeed = tbl_Character_Level.AttackSpeed;
        this.weaponData.attackPower = tbl_Character_Level.AttackPower;
        this.weaponData.attackDistance = tbl_Character_Level.AttackDistance;
        this.weaponData.shootPointCnt = tbl_Character_Level.ShootPointCnt;
        this.weaponData.shootingBulletID = tbl_Character_Level.ShootingBulletID;
    }

    public Transform GetBulletSpawnPoint()
    {
        return this.transform;
    }

    /// <summary>
    /// 回收 放入缓存池
    /// </summary>
    public void Recyle()
    {
        PoolMgr.Instance.RecycleObject(this.gameObject.name, this.gameObject);
    }
}
