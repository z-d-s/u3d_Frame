using UnityEngine;
using UnityEngine.SceneManagement;

public class GameApp : MonoBaseSingleton<GameApp>
{
    [SerializeReference]
    public GameConfig config = new GameConfig();

    public override void Awake()
    {
        base.Awake();

        //日志开关
        LogHelper.enumColorLog = config != null ? config.enumColorLog : EnumColorLog.NONE;

        //初始设置
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
#if UNITY_STANDALONE
        Screen.SetResolution(720, 1280, false);
#elif UNITY_ANDROID || UNITY_IOS
        Screen.fullScreen = true;
#endif

        //游戏框架的初始化
        AppFacade.Instance.StartUp();
        TimeMgr.Instance.StartUp();
        AssetBundleLoadMgr.Instance.LoadManifest();

        //初始化游戏模块
        TableMgr.Instance.StartUp();
        UIMgr.Instance.StartUp();
        GameMgr.Instance.StartUp();
    }

    private void Start()
    {
        //进入游戏
        GameMgr.Instance.EnterGame();
    }

    public void EnterMainScene()
    {
        LogHelper.LogMagenta("=== before enter MainScene ===");
        SceneManager.LoadScene("GameMainScene", LoadSceneMode.Single);
    }

    public void EnterFightingScene()
    {
        LogHelper.LogMagenta("=== before enter FightingScene ===");
        SceneManager.LoadScene("GameFightingScene", LoadSceneMode.Additive);
    }
}
