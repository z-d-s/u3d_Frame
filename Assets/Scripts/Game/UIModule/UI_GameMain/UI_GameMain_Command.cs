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
            case EventDefine.MVC_UI_GameMain_StartUp:
                UI_GameMain_Mediator mediator = GameFacade.Instance.RetrieveMediator(UI_GameMain_Mediator.NAME) as UI_GameMain_Mediator;
                GameObject parent = notification.Body as GameObject;

                if (mediator == null)
                {
                    AssetsLoadMgr.Instance.LoadAsync("ui_gamemain", "UI/Prefabs/UI_GameMain.prefab", (_assetName, _obj) =>
                    {
                        GameObject ui_view = GameObject.Instantiate(_obj as GameObject);
                        ui_view.name = _obj.name;
                        if (parent == null)
                        {
                            parent = UIMgr.Instance.ui_Down.gameObject;
                        }
                        ui_view.transform.SetParent(parent.transform, false);
                        ui_view.SetActive(false);

                        GameFacade.Instance.RegisterMediator(new UI_GameMain_Mediator(UI_GameMain_Mediator.NAME, ui_view.AddComponent(Type.GetType(ui_view.name))));
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

    UI_GameMain_Proxy dataProxy
    {
        get
        {
            return GameFacade.Instance.RetrieveProxy(UI_GameMain_Proxy.NAME) as UI_GameMain_Proxy;
        }
    }
}
