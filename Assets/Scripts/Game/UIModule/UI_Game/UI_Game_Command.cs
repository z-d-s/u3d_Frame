using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using System;
using UnityEngine;

public class UI_Game_Command : SimpleCommand
{
    public override void Execute(INotification notification)
    {
        base.Execute(notification);

        switch(notification.Name)
        {
            case EventDefine.MVC_UI_Game_StartUp:
                UI_Game_Mediator mediator = AppFacade.Instance.RetrieveMediator(UI_Game_Mediator.NAME) as UI_Game_Mediator;
                GameObject parent = notification.Body as GameObject;

                if (mediator == null)
                {
                    AssetsLoadMgr.Instance.LoadAsync("ui_game", "UI/Prefabs/UI_Game.prefab", (_assetName, _obj) =>
                    {
                        GameObject ui_view = GameObject.Instantiate(_obj as GameObject);
                        ui_view.name = _obj.name;
                        if (parent == null)
                        {
                            parent = UIMgr.Instance.canvas.gameObject;
                        }
                        ui_view.transform.SetParent(parent.transform, false);
                        ui_view.SetActive(false);

                        AppFacade.Instance.RegisterMediator(new UI_Game_Mediator(UI_Game_Mediator.NAME, ui_view.AddComponent(Type.GetType(ui_view.name))));
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

    UI_Game_Proxy dataProxy
    {
        get
        {
            return AppFacade.Instance.RetrieveProxy(UI_Game_Proxy.NAME) as UI_Game_Proxy;
        }
    }
}
