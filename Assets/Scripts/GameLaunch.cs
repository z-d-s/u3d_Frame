using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameLaunch : MonoBaseSingleton<GameLaunch>
{
    public GameConfig config;

    public override void Awake()
    {
        base.Awake();

        #region 游戏框架的初始化
        this.gameObject.AddComponent<ResMgr>();
        #endregion

        #region 初始化游戏模块，然后进入游戏
        this.gameObject.AddComponent<GameApp>();
        #endregion
    }

    private void Start()
    {
        GameApp.Instance.EnterGame();
    }
}
