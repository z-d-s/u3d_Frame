using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameApp : UnitySingleton<GameApp>
{
    public void EnterGame()
    {
        this.EnterFightingScene();
    }

    public void EnterFightingScene()
    {
        #region 获取战斗的地图
        //GameObject mapPrefab = ResMgr.Instance.GetAssetCache<GameObject>("Maps/sgyd/SGYD.prefab");
        //GameObject map = GameObject.Instantiate(mapPrefab);
        //map.AddComponent<GameMgr>().InitGame();
        #endregion

        #region 显示UI
        UI_manager.Instance.ShowUIView("GameUI");
        #endregion
    }
}
