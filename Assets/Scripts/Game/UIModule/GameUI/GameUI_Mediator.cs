using PureMVC.Interfaces;
using PureMVC.Patterns.Mediator;
using System;
using System.Xml.Linq;
using UnityEditor;
using UnityEngine;

public class GameUI_Mediator : Mediator
{
    public new const string NAME = "GameUI_Mediator";
    public GameUI view;

    public GameUI_Mediator(string mediatorName, object viewComponent = null) : base(mediatorName, viewComponent)
    {
        if(viewComponent != null)
        {
            this.view = viewComponent as GameUI;
        }
    }

    public override string[] ListNotificationInterests()
    {
        string[] notifies =
        {
            EventDefine.MVC_GameUI_FillInfo,
            EventDefine.MVC_GameUI_FillInfo,
            EventDefine.MVC_GameUI_Change_Exp,
            EventDefine.MVC_GameUI_Hide,
        };
        return notifies;
    }

    public override void HandleNotification(INotification notification)
    {
        base.HandleNotification(notification);

        switch(notification.Name)
        {
            case EventDefine.MVC_GameUI_FillInfo:
                this.view.FillDataInfo();
                break;
            case EventDefine.MVC_GameUI_Change_Hp:
                this.view.RefreshDataInfo(notification.Body as GameUIData);
                break;
            case EventDefine.MVC_GameUI_Change_Exp:
                this.view.RefreshDataInfo(notification.Body as GameUIData);
                break;
            case EventDefine.MVC_GameUI_Hide:
                this.Hide();
                break;
            default:
                break;
        }
    }

    public override void OnRegister()
    {
        base.OnRegister();

        this.Show();
    }

    public void Show(GameObject parent = null)
    {
        if(this.view)
        {
            if (parent != null)
            {
                this.view.transform.SetParent(parent.transform, false);
            }
            this.view.Show();
        }
    }

    public void Hide()
    {
        if (this.view)
        {
            this.view.Hide();
        }
    }
}
