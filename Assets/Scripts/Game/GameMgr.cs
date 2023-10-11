using UnityEngine.SceneManagement;

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
    public CameraCtrl cameraCtrl = null;
    public ReferenceBall_Target ball_Target;

    public void StartUp()
    {
        LogHelper.LogGreen("=== GameMgr 启动成功 ===");
    }

    /// <summary>
    /// 进入游戏
    /// </summary>
    public void EnterGame()
    {
        if(GameApp.Instance.config.showFPS)
        {
            GameFacade.Instance.SendNotification(EventDefine.MVC_UI_FPS_StartUp, UIMgr.Instance.ui_FPS);
        }

        // 显示UI_Loading界面
        GameFacade.Instance.SendNotification(EventDefine.MVC_UI_Loading_StartUp);
    }

    public void GameOver()
    {
        // 显示UI_GameOver界面
        GameFacade.Instance.SendNotification(EventDefine.MVC_UI_GameOver_StartUp);
    }

    public void EnterMainScene()
    {
        LogHelper.LogMagenta("=== before enter MainScene ===");
        SceneManager.LoadScene("GameMainScene", LoadSceneMode.Single);
    }

    public void EnterFightingScene()
    {
        LogHelper.LogMagenta("=== before enter FightingScene ===");
        SceneManager.LoadScene("GameFightingScene", LoadSceneMode.Single);
    }
}
