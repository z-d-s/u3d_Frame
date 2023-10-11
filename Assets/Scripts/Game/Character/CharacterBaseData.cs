public enum Enum_TeamType
{
    Unknown = 0,
    Self,
    Team,
    Enemy
}

public class CharacterBaseData
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
    /// 角色描述
    /// </summary>
    public string describe = "";

    /********** 以下属性值随等级变化 **********/

    /// <summary>
    /// 移动速度
    /// </summary>
    public float moveSpeed = 0f;
    /// <summary>
    /// 旋转速度
    /// </summary>
    public float rotateSpeed = 0f;
    /// <summary>
    /// 血量
    /// </summary>
    public float hp = 0f;
}