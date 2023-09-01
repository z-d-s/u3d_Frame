using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;
using UnityEngine;

public class UI_Loading_Command : SimpleCommand
{
    public override void Execute(INotification notification)
    {
        base.Execute(notification);

        switch (notification.Name)
        {
            case EventDefine.MVC_UI_Loading_StartUp:
                UI_Loading_Mediator mediator = AppFacade.Instance.RetrieveMediator(UI_Loading_Mediator.NAME) as UI_Loading_Mediator;
                GameObject parent = notification.Body as GameObject;

                if (mediator == null)
                {
                    AssetsLoadMgr.Instance.LoadAsync("ui_loading", "UI/Prefabs/UI_Loading.prefab", (_assetName, _obj) =>
                    {
                        GameObject ui_view = GameObject.Instantiate(_obj as GameObject);
                        ui_view.name = _obj.name;
                        if (parent == null)
                        {
                            parent = UIMgr.Instance.ui_Mid.gameObject;
                        }
                        ui_view.transform.SetParent(parent.transform, false);
                        ui_view.SetActive(false);

                        AppFacade.Instance.RegisterMediator(new UI_Loading_Mediator(UI_Loading_Mediator.NAME, ui_view.AddComponent(Type.GetType(ui_view.name))));
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
}
