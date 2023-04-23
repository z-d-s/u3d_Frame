using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMainScene : MonoBehaviour
{
    private void Awake()
    {
        LogHelper.LogMagenta("=== enter MainScene success ===");
    }

    private void Start()
    {
        this.InitFightingScene();
        this.InitRole();
    }

    private void OnDestroy()
    {
        LogHelper.LogMagenta("=== leave MainScene ===");
    }

    /// <summary>
    /// 初始化战斗地图
    /// </summary>
    public void InitFightingScene()
    {
        AssetsLoadMgr.Instance.LoadAsync("terrain_a000", "Models/Prefabs/Maps/TerrainA000.prefab", (string name, UnityEngine.Object obj) =>
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
        #region 放置角色player
        AssetsLoadMgr.Instance.LoadAsync("role_max", "Characters/Prefabs/Role_Max.prefab", (string name, UnityEngine.Object obj) =>
        {
            GameObject player = GameObject.Instantiate(obj as GameObject);
            player.name = "player";
            player.transform.position = new Vector3(0f, 0f, 0f);
            player.transform.localScale = Vector3.one * 3f;
            player.AddComponent<PlayerOpt>().Init();
            GameMgr.Instance.characterMain = player.AddComponent<CharacterMain>();
            GameMgr.Instance.characterMain.Init();
        });
        #endregion
    }

    public void ResetGame()
    {

    }
}
