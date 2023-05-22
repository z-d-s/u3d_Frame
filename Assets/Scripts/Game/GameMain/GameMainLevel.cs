using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameMainLevel : MonoBehaviour
{
    private void Awake()
    {
        LogHelper.LogMagenta("=== enter MainScene success ===");

        // 显示UI_Game界面
        AppFacade.Instance.SendNotification(EventDefine.MVC_UI_Game_StartUp);
    }

    private void Start()
    {
        this.InitFightingScene();
    }

    private void OnDestroy()
    {
        LogHelper.LogMagenta("=== leave MainScene ===");

        // 隐藏UI_Game界面
        AppFacade.Instance.SendNotification(EventDefine.MVC_UI_Game_Hide);
    }

    /// <summary>
    /// 初始化战斗地图
    /// </summary>
    public void InitFightingScene()
    {
        AssetsLoadMgr.Instance.LoadAsync("map_foundation", "Maps/Prefabs/Foundation_001.prefab", (string name, UnityEngine.Object obj) =>
        {
            GameObject foundationObj = PoolMgr.Instance.GetObject(obj.name, obj as GameObject);
            FoundationNode node = new FoundationNode();
            node.nodeObj = foundationObj;
            node.SetPos(Vector3.zero);
            node.Init(2f, 2f);

            GameObject foundationObj_02 = PoolMgr.Instance.GetObject(obj.name, obj as GameObject);
            FoundationNode node_02 = new FoundationNode();
            node_02.nodeObj = foundationObj_02;

            node.SetNextNode(node_02);
            node_02.SetOffsetPos(new Vector3(0, 0, 6));
            node_02.Init(2f, 2f);

            this.InitRole(node);
        });
    }

    /// <summary>
    /// 初始化角色
    /// </summary>
    public void InitRole(FoundationNode node)
    {
        #region 放置角色player
        AssetsLoadMgr.Instance.LoadAsync("role_jinglingnan", "Characters/Jinglingnan/Prefabs/JingLingNan.prefab", (string name, UnityEngine.Object obj) =>
        {
            GameObject player = GameObject.Instantiate(obj as GameObject);
            player.name = "player";
            player.transform.position = new Vector3(0, 0, 0);
            player.transform.localScale = Vector3.one;
            GameMgr.Instance.characterMain = player.AddComponent<CharacterMain>();
            GameMgr.Instance.characterMain.Init();
            GameMgr.Instance.characterMain.currentFoundationNode = node;
            GameMgr.Instance.firstFoundationNode = node;

            this.InitMainCamera();
        });
        #endregion
    }

    /// <summary>
    /// 初始化主摄像机
    /// </summary>
    public void InitMainCamera()
    {
        GameMgr.Instance.cameraCtrl = Camera.main.transform.AddComponent<CameraCtrl>();
        GameMgr.Instance.cameraCtrl.Init();
    }
}
