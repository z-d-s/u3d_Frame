using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDefine
{
    public const string None                            = "";

    public const string MVC_UI_Loading_StartUp          = "UI_Loading_StartUp";
    public const string MVC_UI_Loading_FillInfo         = "UI_Loading_FillInfo";
    public const string MVC_UI_Loading_Hide             = "UI_Loading_Hide";

    public const string MVC_UI_Game_StartUp             = "UI_Game_StartUp";
    public const string MVC_UI_Game_FillInfo            = "UI_Game_FillInfo";
    public const string MVC_UI_Game_Change_Hp           = "UI_Game_Change_Hp";
    public const string MVC_UI_Game_Change_Score        = "UI_Game_Change_Score";
    public const string MVC_UI_Game_RefreshTouchingTime = "UI_Game_RefreshTouchingTime";
    public const string MVC_UI_Game_Hide                = "UI_Game_Hide";
    public const string EVE_Game_RefreshTargetPos       = "Game_RefreshTargetPos";
    public const string EVE_Game_Jump                   = "Game_Jump";
    public const string EVE_Game_SetNextNode            = "Game_SetNextNode";

    public const string MVC_UI_Setting_StartUp          = "UI_Setting_StartUp";
    public const string MVC_UI_Setting_FillInfo         = "UI_Setting_FillInfo";
    public const string MVC_UI_Setting_Hide             = "UI_Setting_Hide";

    public const string MVC_UI_GameOver_StartUp         = "UI_GameOver_StartUp";
    public const string MVC_UI_GameOver_FillInfo        = "UI_GameOver_FillInfo";
    public const string MVC_UI_GameOver_Hide            = "UI_GameOver_Hide";
    public const string EVE_GameRestart                 = "UI_GameRestart";
}