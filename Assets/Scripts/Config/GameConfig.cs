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

public enum EnumFPS
{
    UnLock = -1,
    Lock_30 = 30,
    Lock_60 = 60,
}

public class GameConfig
{
    public bool showFPS = false;

    public EnumLoadType loadType = EnumLoadType.LocalRes;

    public EnumColorLog enumColorLog = EnumColorLog.NONE;

    public EnumFPS enumFPS = EnumFPS.Lock_60;
}
