using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using UnityEngine;

public class GameUI_Command : SimpleCommand
{
    private string assetBundleName = "ui_gameui";
    private string uiName = "GameUI";

    public override void Execute(INotification notification)
    {
        base.Execute(notification);

        GameUI_Mediator mediator = AppFacade.Instance.RetrieveMediator(GameUI_Mediator.NAME) as GameUI_Mediator;

        switch(notification.Name)
        {
            case GameEventDefine.GAMEUI_STARTUP:
                if (mediator == null)
                {
                    string assetName = "GUI/UIPrefabs/" + this.uiName + ".prefab";
                    AssetsLoadMgr.Instance.LoadAsync(this.assetBundleName, assetName, (string name, UnityEngine.Object obj) =>
                    {
                        GameObject ui_view = GameObject.Instantiate(obj as GameObject);
                        ui_view.name = obj.name;
                        ui_view.transform.SetParent(UIMgr.Instance.canvas, false);

                        AppFacade.Instance.RegisterMediator(new GameUI_Mediator(GameUI_Mediator.NAME, ui_view.AddComponent<GameUI>()));
                    });
                }
                else
                {
                    mediator.Show();
                }
                break;
            default:
                break;
        }
    }

    GameUI_Proxy dataProxy
    {
        get
        {
            return Facade.RetrieveProxy(GameUI_Proxy.NAME) as GameUI_Proxy;
        }
    }
}
