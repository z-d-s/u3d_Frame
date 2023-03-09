using PureMVC.Patterns.Facade;
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
        AssetsLoadMgr.Instance.LoadAsync("role_jinglingnan", "Charactors/Monsters/Jinglingnan_6.prefab", (string name, UnityEngine.Object obj) =>
        {
            GameObject charactorPrefab = obj as GameObject;
            this.player = GameObject.Instantiate(charactorPrefab);
            this.player.AddComponent<CharactorCtrl>().Init();
            this.player.AddComponent<PlayerOpt>().Init();
            this.player.name = "player";
        });
        #endregion

        #region 放置敌人
        AssetsLoadMgr.Instance.LoadAsync("role_jinglingnan", "Charactors/Monsters/Jinglingnan_6.prefab", (string name, UnityEngine.Object obj) =>
        {
            GameObject charactorPrefab = obj as GameObject;
            GameObject e = GameObject.Instantiate(charactorPrefab);
            this.enemies.Add(e);
            e.transform.position = this.player.transform.position + new Vector3(2, 0, 2);
            e.AddComponent<CharactorCtrl>().Init();
            e.name = "enemy";
        });
        #endregion
    }

    private void OnGUI()
    {
        if(GUI.Button(new Rect(10, 10, 150, 50), "Add Role"))
        {
            //AssetsLoadMgr.Instance.LoadAsync("role_jinglingnan", "Charactors/Monsters/Jinglingnan_6.prefab", (string name, UnityEngine.Object obj) =>
            //{
            //    GameObject charactorPrefab = obj as GameObject;
            //    GameObject e = GameObject.Instantiate(charactorPrefab);
            //    this.enemies.Add(e);
            //    e.transform.position = this.player.transform.position + new Vector3(3, 0, 3);
            //    e.AddComponent<CharactorCtrl>().Init();
            //    e.name = "enemy";
            //});

            AppFacade.GetInstance("", key => new Facade(key)).SendNotification(EnumGameNotify.GAMEUI_STARTUP.ToString());
        }

        if (GUI.Button(new Rect(10, 70, 150, 50), "Add Arrow"))
        {
            //AssetsLoadMgr.Instance.LoadAsync("effect_asset", "Effects/Prefabs/arrows.prefab", (string name, UnityEngine.Object obj) =>
            //{
            //    GameObject charactorPrefab = obj as GameObject;
            //    GameObject e = GameObject.Instantiate(charactorPrefab);
            //    e.transform.position = this.player.transform.position + new Vector3(1, 0, 1);
            //    e.name = "arrow";
            //});
        }

        if (GUI.Button(new Rect(170, 70, 150, 50), "Add Sword"))
        {
            AssetsLoadMgr.Instance.LoadAsync("effect_asset", "Effects/Prefabs/swords.prefab", (string name, UnityEngine.Object obj) =>
            {
                GameObject charactorPrefab = obj as GameObject;
                GameObject e = GameObject.Instantiate(charactorPrefab);
                e.transform.position = this.player.transform.position + new Vector3(1, 0, 1);
                e.name = "sword";
            });
        }
    }
}
