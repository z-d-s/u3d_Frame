using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    None,
    Gameing,
    Jumping,
    Death,
    GameOver
}

public class GameMgr : BaseSingleton<GameMgr>
{
    public GameState gameState = GameState.None;
    public FoundationNode firstFoundationNode;
    public CharacterMain characterMain = null;
    public CameraCtrl cameraCtrl = null;
    public ReferenceBall_Target ball_Target;
    public ReferenceBall_Calibration ball_Calibration;

    public void StartUp()
    {
        LogHelper.LogGreen("=== GameMgr 启动成功 ===");
    }

    /// <summary>
    /// 进入游戏
    /// </summary>
    public void EnterGame()
    {
        // 显示UI_Loading界面
        AppFacade.Instance.SendNotification(EventDefine.MVC_UI_Loading_StartUp);
    }

    public void GameOver()
    {
        // 显示UI_GameOver界面
        AppFacade.Instance.SendNotification(EventDefine.MVC_UI_GameOver_StartUp);
    }
}
