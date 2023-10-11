using Unity.VisualScripting;
using UnityEngine;

public class GameFightingScene : MonoBehaviour
{
    private GameObject obj_CurrentMap;

    private void Awake()
    {
        LogHelper.LogMagenta("=== enter FightingScene success ===");
    }

    private void Start()
    {
        this.InitFightingScene();
    }

    private void OnDestroy()
    {
        LogHelper.LogMagenta("=== leave FightingScene ===");

        // 隐藏UI_GameControl界面
        GameFacade.Instance.SendNotification(EventDefine.MVC_UI_GameControl_Hide);
    }

    /// <summary>
    /// 初始化战斗地图
    /// </summary>
    private void InitFightingScene()
    {
        if(!TableMgr.Instance.ExistTableID<Tbl_Map>("3000"))
        {
            return;
        }

        Tbl_Map tbl_Map = TableMgr.Instance.GetTable<Tbl_Map>("3000");
        AssetsLoadMgr.Instance.LoadAsync(tbl_Map.AssetBundleName, tbl_Map.AssetNamePath, (string _assetName, UnityEngine.Object _obj) =>
        {
            GameObject map = GameObject.Instantiate(_obj as GameObject);
            map.transform.position = Vector3.zero;
            map.transform.localScale = Vector3.one;

            this.obj_CurrentMap = map;
            this.InitRole();
        });
    }

    /// <summary>
    /// 初始化角色
    /// </summary>
    public void InitRole()
    {
        string characterID = "1001";
        int characterLevel = 1;
        CharacterMgr.Instance.CreateCharacter(characterID, characterLevel, (CharacterMain character) =>
        {
            if(character == null)
            {
                return;
            }

            CharacterMgr.Instance.mainCharacter = character;
            character.Init(characterLevel, Enum_TeamType.Self, this.obj_CurrentMap.transform.Find("Map_SpawnPoints/point_main").position);

            this.InitReferenceBall_Calibration();
        });
    }

    /// <summary>
    /// 初始化 标定参考球
    /// </summary>
    public void InitReferenceBall_Calibration()
    {
        this.InitMainCamera();

        //AssetsLoadMgr.Instance.LoadAsync("gizmos_ball", "GizmosModels/Prefabs/Gizmos_Ball.prefab", (string _name, UnityEngine.Object _obj) =>
        //{
        //    GameObject ball = GameObject.Instantiate(_obj as GameObject);

        //    this.InitMainCamera();
        //});
    }

    /// <summary>
    /// 初始化主摄像机
    /// </summary>
    public void InitMainCamera()
    {
        GameMgr.Instance.cameraCtrl = Camera.main.transform.AddComponent<CameraCtrl>();
        GameMgr.Instance.cameraCtrl.Init();

        // 显示UI_GameControl界面
        GameFacade.Instance.SendNotification(EventDefine.MVC_UI_GameControl_StartUp);
        //this.InitReferenceBall_Target();
    }

    /// <summary>
    /// 初始化 前面参考球
    /// </summary>
    public void InitReferenceBall_Target()
    {
        AssetsLoadMgr.Instance.LoadAsync("gizmos_ball", "GizmosModels/Prefabs/Gizmos_Ball.prefab", (string name, UnityEngine.Object obj) =>
        {
            GameObject ball = GameObject.Instantiate(obj as GameObject);
            GameMgr.Instance.ball_Target = ball.AddComponent<ReferenceBall_Target>();
            GameMgr.Instance.ball_Target.SetPos(CharacterMgr.Instance.mainCharacter.GetCharacterPos());
        });
    }
}
