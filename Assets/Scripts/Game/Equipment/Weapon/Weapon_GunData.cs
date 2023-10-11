public class Weapon_GunData
{
    /// <summary>
    /// id
    /// </summary>
    public string id = "";
    /// <summary>
    /// 名称
    /// </summary>
    public string name = "";
    /// <summary>
    /// 武器描述
    /// </summary>
    public string describe = "";

    /********** 以下属性值随等级变化 **********/

    /// <summary>
    /// 攻击速度
    /// </summary>
    public float attackSpeed = 0f;
    /// <summary>
    /// 攻击力
    /// </summary>
    public float attackPower = 0f;
    /// <summary>
    /// 攻击距离
    /// </summary>
    public float attackDistance = 0f;
    /// <summary>
    /// 攻击口数量
    /// </summary>
    public int shootPointCnt = 0;
    /// <summary>
    /// 使用子弹ID
    /// </summary>
    public string shootingBulletID;
}