using System.Collections.Generic;
using UnityEngine;

public class GameMgr : BaseSingleton<GameMgr>
{
    public GameObject player = null;
    public List<GameObject> enemies = new List<GameObject>();

    public List<GameObject> FindCharactorsInRadius(Vector3 o, float radius)
    {
        //四叉树 场景管理

        //九宫格 场景管理

        return enemies;
    }

    public void StartUp()
    {
        UtilLog.LogGreen("=== GameMgr 启动成功 ===");
    }

    /// <summary>
    /// 进入游戏
    /// </summary>
    public void EnterGame()
    {
        #region 显示UI
        AppFacade.Instance.SendNotification(EventDefine.MVC_GameUI_StartUp);
        #endregion

        this.InitFightingScene();
        this.InitRole();
    }

    /// <summary>
    /// 初始化战斗地图
    /// </summary>
    public void InitFightingScene()
    {
        AssetsLoadMgr.Instance.LoadAsync("scene_sgyd", "Maps/Prefabs/SGYD.prefab", (string name, UnityEngine.Object obj) =>
        {
            GameObject map = GameObject.Instantiate(obj as GameObject);
            map.name = obj.name;
        });
    }

    /// <summary>
    /// 初始化角色
    /// </summary>
    public void InitRole()
    {
        #region 放置NPC
        #endregion

        #region 放置角色player
        AssetsLoadMgr.Instance.LoadAsync("role_jinglingnan", "Charactors/Prefabs/Jinglingnan_6.prefab", (string name, UnityEngine.Object obj) =>
        {
            GameObject charactorPrefab = obj as GameObject;
            this.player = GameObject.Instantiate(charactorPrefab);
            this.player.transform.position = new Vector3(45f, 60.25f, 100f);
            this.player.transform.localScale = Vector3.one * 1.5f;
            this.player.AddComponent<CharactorCtrl>().Init();
            this.player.AddComponent<PlayerOpt>().Init();
            this.player.name = "player";
        });
        #endregion

        #region 放置敌人
        AssetsLoadMgr.Instance.LoadAsync("role_jinglingnan", "Charactors/Prefabs/Jinglingnan_6.prefab", (string name, UnityEngine.Object obj) =>
        {
            GameObject charactorPrefab = obj as GameObject;
            GameObject e = GameObject.Instantiate(charactorPrefab);
            this.enemies.Add(e);
            e.transform.position = this.player.transform.position + new Vector3(2, 0, 2);
            e.transform.localScale = Vector3.one * 1.5f;
            e.AddComponent<CharactorCtrl>().Init();
            e.name = "enemy";
        });
        #endregion
    }
}
