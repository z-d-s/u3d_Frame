using Unity.VisualScripting;
using UnityEngine;

public class GameMainLevel : MonoBehaviour
{
    private void Awake()
    {
        LogHelper.LogMagenta("=== enter MainScene success ===");
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
        LogHelper.LogBlue("需要新的基座01");
        GameObject foundationObj = PoolMgr.Instance.GetObject("map_foundation", "Maps/Prefabs/Foundation_001.prefab");
        FoundationNode node = new FoundationNode();
        node.nodeObj = foundationObj;
        node.SetPos(Vector3.zero);
        node.Init(2f, 2f);

        this.InitRole(node);
    }

    /// <summary>
    /// 初始化角色
    /// </summary>
    public void InitRole(FoundationNode node)
    {
        AssetsLoadMgr.Instance.LoadAsync("role_jinglingnan", "Characters/Jinglingnan/Prefabs/JingLingNan.prefab", (string name, UnityEngine.Object obj) =>
        {
            GameObject player = GameObject.Instantiate(obj as GameObject);
            player.name = "player";
            player.transform.position = Vector3.zero;
            player.transform.localScale = Vector3.one;
            GameMgr.Instance.characterMain = player.AddComponent<CharacterMain>();
            GameMgr.Instance.characterMain.Init();
            GameMgr.Instance.characterMain.currentFoundationNode = node;
            GameMgr.Instance.firstFoundationNode = node;

            this.InitReferenceBall_Calibration();
        });
    }

    /// <summary>
    /// 初始化 标定参考球
    /// </summary>
    public void InitReferenceBall_Calibration()
    {
        AssetsLoadMgr.Instance.LoadAsync("map_foundation", "Maps/Prefabs/ReferenceBall.prefab", (string name, UnityEngine.Object obj) =>
        {
            GameObject ball = GameObject.Instantiate(obj as GameObject);
            GameMgr.Instance.ball_Calibration = ball.AddComponent<ReferenceBall_Calibration>();
            GameMgr.Instance.ball_Calibration.SetTargetNode(GameMgr.Instance.characterMain.currentFoundationNode);
            GameMgr.Instance.ball_Calibration.SetTargetPos(GameMgr.Instance.characterMain.currentFoundationNode.GetPos(), 0f);

            this.InitMainCamera();
        });
    }

    /// <summary>
    /// 初始化主摄像机
    /// </summary>
    public void InitMainCamera()
    {
        GameMgr.Instance.cameraCtrl = Camera.main.transform.AddComponent<CameraCtrl>();
        GameMgr.Instance.cameraCtrl.Init();

        // 显示UI_Game界面
        AppFacade.Instance.SendNotification(EventDefine.MVC_UI_Game_StartUp);
        this.InitReferenceBall_Target();
    }

    /// <summary>
    /// 初始化 前面参考球
    /// </summary>
    public void InitReferenceBall_Target()
    {
        AssetsLoadMgr.Instance.LoadAsync("map_foundation", "Maps/Prefabs/ReferenceBall.prefab", (string name, UnityEngine.Object obj) =>
        {
            GameObject ball = GameObject.Instantiate(obj as GameObject);
            GameMgr.Instance.ball_Target = ball.AddComponent<ReferenceBall_Target>();
            GameMgr.Instance.ball_Target.SetPos(GameMgr.Instance.characterMain.GetCharacterPos());
        });
    }
}
