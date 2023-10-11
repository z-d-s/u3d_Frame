using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class UI_Announcement_Command : SimpleCommand
{
    public override void Execute(INotification notification)
    {
        base.Execute(notification);

        switch(notification.Name)
        {
            case EventDefine.MVC_UI_Announcement_StartUp:
                UI_Announcement_Mediator mediator = GameFacade.Instance.RetrieveMediator(UI_Announcement_Mediator.NAME) as UI_Announcement_Mediator;
                GameObject parent = notification.Body.ConvertTo<GameObject>();

                if (mediator == null)
                {
                    AssetsLoadMgr.Instance.LoadAsync("ui_announcement", "UI/Prefabs/UI_Announcement.prefab", (_assetName, _obj) =>
                    {
                        GameObject ui_view = GameObject.Instantiate(_obj as GameObject);
                        ui_view.name = _obj.name;
                        if (parent == null)
                        {
                            parent = UIMgr.Instance.ui_Down.gameObject;
                        }
                        ui_view.transform.SetParent(parent.transform, false);
                        ui_view.SetActive(false);

                        GameFacade.Instance.RegisterMediator(new UI_Announcement_Mediator(UI_Announcement_Mediator.NAME, ui_view.AddComponent(Type.GetType(ui_view.name))));
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
