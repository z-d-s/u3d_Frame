using JetBrains.Annotations;
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
        Application.targetFrameRate = (int)config.enumFPS;
#if UNITY_STANDALONE
        Screen.SetResolution(720, 1280, false);
#elif UNITY_ANDROID || UNITY_IOS
        Screen.fullScreen = true;
#endif

        //游戏框架的初始化
        AssetBundleLoadMgr.Instance.LoadManifest();
        GameFacade.Instance.StartUp();

        //初始化游戏模块
        TimeMgr.Instance.StartUp();
        UIMgr.Instance.StartUp();
        LanguageData.FillLanguageData(true);
        TableMgr.Instance.StartUp();
        GameMgr.Instance.StartUp();
    }

    private void Start()
    {
        //进入游戏
        GameMgr.Instance.EnterGame();
    }
}
