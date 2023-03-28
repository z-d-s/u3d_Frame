using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameApp : MonoBaseSingleton<GameApp>
{
    public GameConfig config;

    public override void Awake()
    {
        base.Awake();

        #region 游戏框架的初始化
        AppFacade.Instance.StartUp();
        AssetBundleLoadMgr.Instance.LoadManifest();
        #endregion

        #region 初始化游戏模块
        UtilLog.enumColorLog = config != null ? config.enumColorLog : EnumColorLog.NONE;
        UIMgr.Instance.Init();
        GameMgr.Instance.Init();
        #endregion
    }

    private void Start()
    {
        #region 进入游戏
        GameMgr.Instance.EnterGame();
        #endregion
    }
}
