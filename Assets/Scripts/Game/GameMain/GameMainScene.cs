using Unity.VisualScripting;
using UnityEngine;

public class GameMainScene : MonoBehaviour
{
    private void Awake()
    {
        LogHelper.LogMagenta("=== enter MainScene success ===");
    }

    private void Start()
    {
        this.InitMainScene();
    }

    private void OnDestroy()
    {
        LogHelper.LogMagenta("=== leave MainScene ===");

        // 隐藏UI_GameMain界面
        GameFacade.Instance.SendNotification(EventDefine.MVC_UI_GameMain_Hide);
    }

    private void InitMainScene()
    {
        GameFacade.Instance.SendNotification(EventDefine.MVC_UI_GameMain_StartUp);
    }
}
