using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using System;
using UnityEngine;

public class GameUI_Command : SimpleCommand
{
    public override void Execute(INotification notification)
    {
        base.Execute(notification);

        switch(notification.Name)
        {
            case EventDefine.MVC_GameUI_StartUp:
                GameUI_Mediator mediator = AppFacade.Instance.RetrieveMediator(GameUI_Mediator.NAME) as GameUI_Mediator;
                GameObject parent = notification.Body as GameObject;

                if (mediator == null)
                {
                    AssetsLoadMgr.Instance.LoadAsync("ui_gameui", "UI/Prefabs/GameUI.prefab", (_assetName, _obj) =>
                    {
                        GameObject ui_view = GameObject.Instantiate(_obj as GameObject);
                        ui_view.name = _obj.name;
                        if (parent == null)
                        {
                            parent = UIMgr.Instance.canvas.gameObject;
                        }
                        ui_view.transform.SetParent(parent.transform, false);
                        ui_view.SetActive(false);

                        AppFacade.Instance.RegisterMediator(new GameUI_Mediator(GameUI_Mediator.NAME, ui_view.AddComponent(Type.GetType(ui_view.name))));
                    });
                }
                else
                {
                    mediator.Show(parent);
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
            return AppFacade.Instance.RetrieveProxy(GameUI_Proxy.NAME) as GameUI_Proxy;
        }
    }
}
