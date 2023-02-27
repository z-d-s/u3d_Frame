using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameApp : MonoBaseSingleton<GameApp>
{
    public void EnterGame()
    {
        this.EnterFightingScene();
    }

    public void EnterFightingScene()
    {
        #region 获取战斗的地图
        //AssetsLoadMgr.Instance.LoadAsync("Maps/sgyd/SGYD.prefab", (string name, UnityEngine.Object obj) =>
        //{
        //    GameObject map = GameObject.Instantiate(obj as GameObject);
        //    map.AddComponent<GameMgr>().InitGame();
        //});

        AssetBundle ab = AssetBundle.LoadFromFile(FileHelper.BaseLocalResPath() + FileHelper.ABManiName + "/scene_sgyd");
        GameObject map = GameObject.Instantiate(ab.LoadAsset<GameObject>("SGYD"));
        map.AddComponent<GameMgr>().InitGame();
        #endregion

        #region 显示UI
        UI_manager.Instance.ShowUIView("GameUI");
        #endregion
    }
}
