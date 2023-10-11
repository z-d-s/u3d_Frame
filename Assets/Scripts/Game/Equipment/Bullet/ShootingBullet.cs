using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingBullet : MonoBehaviour
{
    /// <summary>
    /// 子弹飞行速度
    /// </summary>
    public float speed = 0f;

    /// <summary>
    /// 武器持有者
    /// </summary>
    public CharacterBase owner;

    private Vector3 flyDir = Vector3.zero;

    public void Init(CharacterBase owner, Transform spawnPoint, Vector3 dir)
    {
        this.owner = owner;
        this.transform.position = spawnPoint.position;
        this.transform.rotation = this.owner.GetCharacterWorldRot();

        this.flyDir = dir;

        TimeMgr.Instance.DoOnce(1500, () =>
        {
            this.Recyle();
        });
    }

    private void Update()
    {
        if(this.owner ==  null)
        {
            return;
        }

        this.transform.Translate(this.flyDir *  speed, Space.World);
    }

    /// <summary>
    /// 回收 放入缓存池
    /// </summary>
    public void Recyle()
    {
        this.owner = null;
        PoolMgr.Instance.RecycleObject(this.gameObject.name, this.gameObject);
    }
}
