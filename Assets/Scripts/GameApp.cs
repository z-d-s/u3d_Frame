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
        new AppFacade();
        AssetBundleLoadMgr.Instance.LoadManifest();
        #endregion

        #region 初始化游戏模块
        UIMgr.Instance.Init();
        #endregion
    }

    private void Start()
    {
        #region 进入游戏
        this.EnterGame();
        #endregion
    }

    public void EnterGame()
    {
        this.EnterFightingScene();
    }

    public void EnterFightingScene()
    {
        #region 获取战斗的地图
        AssetsLoadMgr.Instance.LoadAsync("scene_sgyd", "Maps/sgyd/SGYD.prefab", (string name, UnityEngine.Object obj) =>
        {
            GameObject map = GameObject.Instantiate(obj as GameObject);
            map.AddComponent<GameMgr>().InitGame();
        });
        #endregion

        #region 显示UI
        //UI_manager.Instance.ShowUIView("ui_gameui", "GameUI");
        #endregion
    }
}
