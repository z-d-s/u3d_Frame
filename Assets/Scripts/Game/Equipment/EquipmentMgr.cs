using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentMgr : BaseSingleton<EquipmentMgr>
{
    /// <summary>
    /// 创建武器
    /// </summary>
    /// <param name="id">武器ID</param>
    /// <param name="callback">回调(将武器类做为参数返回)</param>
    public void CreateWeapon(string id, System.Action<Weapon_Gun> callback)
    {
        if(!TableMgr.Instance.ExistTableID<Tbl_Weapon>(id))
        {
            callback?.Invoke(null);
            return;
        }

        Tbl_Weapon tbl_Weapon = TableMgr.Instance.GetTable<Tbl_Weapon>(id);
        string weaponName = UtilTool.GetNameFromPath(tbl_Weapon.AssetNamePath);
        GameObject obj_Weapon = PoolMgr.Instance.GetObject(weaponName);

        if (obj_Weapon == null)
        {
            AssetsLoadMgr.Instance.LoadAsync(tbl_Weapon.AssetBundleName, tbl_Weapon.AssetNamePath, (string _assetName, UnityEngine.Object _obj) =>
            {
                obj_Weapon = GameObject.Instantiate(_obj as GameObject);
                obj_Weapon.name = weaponName;
                Weapon_Gun gun = obj_Weapon.AddComponent<Weapon_Gun>();
                gun.weaponData.id = id;
                gun.weaponData.name = tbl_Weapon.Name;
                gun.weaponData.describe = tbl_Weapon.Describe;
                callback?.Invoke(gun);
            });
        }
        else
        {
            Weapon_Gun gun = obj_Weapon.GetComponent<Weapon_Gun>();
            callback?.Invoke(gun);
        }
    }

    /// <summary>
    /// 创建子弹
    /// </summary>
    /// <param name="id">子弹ID</param>
    /// <param name="callback">回调(将子弹类做为参数返回)</param>
    public void CreateBullet(string id, System.Action<ShootingBullet> callback)
    {
        if (!TableMgr.Instance.ExistTableID<Tbl_ShootingBullet>(id))
        {
            callback?.Invoke(null);
            return;
        }

        Tbl_ShootingBullet tbl_ShootingBullet = TableMgr.Instance.GetTable<Tbl_ShootingBullet>(id);
        string bulletName = UtilTool.GetNameFromPath(tbl_ShootingBullet.AssetNamePath);
        GameObject obj_Bullet = PoolMgr.Instance.GetObject(bulletName);

        if(obj_Bullet == null)
        {
            AssetsLoadMgr.Instance.LoadAsync(tbl_ShootingBullet.AssetBundleName, tbl_ShootingBullet.AssetNamePath, (string _assetName, UnityEngine.Object _obj) =>
            {
                obj_Bullet = GameObject.Instantiate(_obj as GameObject);
                obj_Bullet.name = bulletName;
                ShootingBullet bullet = obj_Bullet.AddComponent<ShootingBullet>();
                bullet.speed = tbl_ShootingBullet.Speed;
                callback?.Invoke(bullet);
            });
        }
        else
        {
            ShootingBullet bullet = obj_Bullet.GetComponent<ShootingBullet>();
            callback?.Invoke(bullet);
        }
    }
}
