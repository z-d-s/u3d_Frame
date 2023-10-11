using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class UI_FPS_Command : SimpleCommand
{
    public override void Execute(INotification notification)
    {
        base.Execute(notification);

        switch(notification.Name)
        {
            case EventDefine.MVC_UI_FPS_StartUp:
                UI_FPS_Mediator mediator = GameFacade.Instance.RetrieveMediator(UI_FPS_Mediator.NAME) as UI_FPS_Mediator;
                GameObject parent = notification.Body.ConvertTo<GameObject>();

                if (mediator == null)
                {
                    AssetsLoadMgr.Instance.LoadAsync("ui_fps", "UI/Prefabs/UI_FPS.prefab", (_assetName, _obj) =>
                    {
                        GameObject ui_view = GameObject.Instantiate(_obj as GameObject);
                        ui_view.name = _obj.name;
                        if (parent == null)
                        {
                            parent = UIMgr.Instance.ui_FPS.gameObject;
                        }
                        ui_view.transform.SetParent(parent.transform, false);
                        ui_view.SetActive(false);

                        GameFacade.Instance.RegisterMediator(new UI_FPS_Mediator(UI_FPS_Mediator.NAME, ui_view.AddComponent(Type.GetType(ui_view.name))));
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
