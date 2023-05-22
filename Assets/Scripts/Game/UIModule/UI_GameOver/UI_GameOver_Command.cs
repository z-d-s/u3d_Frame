using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using System;
using UnityEngine;

public class UI_GameOver_Command : SimpleCommand
{
    public override void Execute(INotification notification)
    {
        base.Execute(notification);

        switch(notification.Name)
        {
            case EventDefine.MVC_UI_GameOver_StartUp:
                UI_GameOver_Mediator mediator = AppFacade.Instance.RetrieveMediator(UI_GameOver_Mediator.NAME) as UI_GameOver_Mediator;
                GameObject parent = notification.Body as GameObject;

                if (mediator == null)
                {
                    AssetsLoadMgr.Instance.LoadAsync("ui_gameover", "UI/Prefabs/UI_GameOver.prefab", (_assetName, _obj) =>
                    {
                        GameObject ui_view = GameObject.Instantiate(_obj as GameObject);
                        ui_view.name = _obj.name;
                        if (parent == null)
                        {
                            parent = UIMgr.Instance.ui_Canvas.gameObject;
                        }
                        ui_view.transform.SetParent(parent.transform, false);
                        ui_view.SetActive(false);

                        AppFacade.Instance.RegisterMediator(new UI_GameOver_Mediator(UI_GameOver_Mediator.NAME, ui_view.AddComponent(Type.GetType(ui_view.name))));
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

    UI_GameOver_Proxy dataProxy
    {
        get
        {
            return AppFacade.Instance.RetrieveProxy(UI_GameOver_Proxy.NAME) as UI_GameOver_Proxy;
        }
    }
}
