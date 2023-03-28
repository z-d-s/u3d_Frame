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

    public float assetCacheTime = 5f;

    public string serverUrl = string.Empty;
}
