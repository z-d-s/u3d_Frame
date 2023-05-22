using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

[CreateAssetMenu(fileName = "GameConfig", menuName = "Config/GameConfig", order = 1)]
public class GameConfig : ScriptableObject
{
    public EnumColorLog enumColorLog = EnumColorLog.NONE;

    /// <summary>
    /// 蓄力移动速度
    /// </summary>
    public float energyMoveSpeed = 5f;

    /// <summary>
    /// 跳跃移动速度
    /// </summary>
    public float jumpMoveSpeed = 10f;
}
