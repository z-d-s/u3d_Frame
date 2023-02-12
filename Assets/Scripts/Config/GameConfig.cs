using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnumGameMode
{
    Develop,
    Local_AB,
    Server_AB,
}

[CreateAssetMenu(fileName = "GameConfig", menuName = "zds/GameConfig", order = 1)]
public class GameConfig : ScriptableObject
{
    public EnumGameMode enumGameMode;

    public float assetCacheTime = 5f;

    public string serverUrl = string.Empty;
}
