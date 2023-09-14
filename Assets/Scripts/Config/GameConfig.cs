public enum EnumLoadType
{
    LocalRes,
    AssetBundle
}

public enum EnumColorLog
{
    NONE,

    WHITE,
    RED,
    GREEN,
    BLUE,

    CYAN,
    MAGENTA,
    YELLOW,

    All
}

public class GameConfig
{
    public EnumLoadType loadType = EnumLoadType.LocalRes;

    public EnumColorLog enumColorLog = EnumColorLog.NONE;

    /// <summary>
    /// 蓄力移动速度
    /// </summary>
    public float energyMoveSpeed = 5f;

    /// <summary>
    /// 跳跃移动速度
    /// </summary>
    public float jumpMoveSpeed = 20f;
}
