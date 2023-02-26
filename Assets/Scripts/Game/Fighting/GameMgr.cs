using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : MonoBehaviour
{
    public static GameMgr Instance = null;
    public GameObject player = null;
    public List<GameObject> enemies = new List<GameObject>();

    private void Awake()
    {
        GameMgr.Instance = this;
    }

    public List<GameObject> findCharactorInRadius(Vector3 o, float radius)
    {
        //四叉树 场景管理

        //九宫格 场景管理

        return enemies;
    }

    public void InitGame()
    {
        #region 放置NPC
        #endregion

        #region 放置角色player
        GameObject charactorPrefab = AssetsLoadMgr.Instance.LoadSync("Charactors/Monsters/Jinglingnan_6.prefab") as GameObject;
        this.player = GameObject.Instantiate(charactorPrefab);
        this.player.AddComponent<CharactorCtrl>().Init();
        this.player.AddComponent<PlayerOpt>().Init();
        this.player.name = "player";
        #endregion

        #region 放置敌人
        GameObject e = GameObject.Instantiate(charactorPrefab);
        this.enemies.Add(e);
        e.transform.position = this.player.transform.position + new Vector3(2, 0, 2);
        e.AddComponent<CharactorCtrl>().Init();
        e.name = "enemy";
        #endregion
    }
}
