using System.Collections.Generic;
using UnityEngine;

public class GameMgr : BaseSingleton<GameMgr>
{
    public CharacterMain characterMain = null;
    public CameraCtrl cameraCtrl = null;

    public void StartUp()
    {
        LogHelper.LogGreen("=== GameMgr 启动成功 ===");
    }

    /// <summary>
    /// 进入游戏
    /// </summary>
    public void EnterGame()
    {
        #region 显示UI
        //AppFacade.Instance.SendNotification(EventDefine.MVC_UI_Game_StartUp);
        AppFacade.Instance.SendNotification(EventDefine.MVC_UI_Loading_StartUp);
        #endregion

        TimeMgr.Instance.DoOnce(1000, () =>
        {
            AppFacade.Instance.SendNotification(EventDefine.MVC_UI_Loading_Hide);
            GameApp.Instance.EnterMainScene();
        });
    }
}
