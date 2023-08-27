using PureMVC.Interfaces;
using PureMVC.Patterns.Mediator;
using System;
using System.Xml.Linq;
using UnityEditor;
using UnityEngine;

public class UI_Setting_Mediator : Mediator
{
    public new const string NAME = "UI_Setting_Mediator";
    public UI_Setting view;

    public UI_Setting_Mediator(string mediatorName, object viewComponent = null) : base(mediatorName, viewComponent)
    {
        if (viewComponent != null)
        {
            this.view = viewComponent as UI_Setting;
        }
    }

    public override string[] ListNotificationInterests()
    {
        string[] notifies =
        {
            EventDefine.MVC_UI_Setting_FillInfo,
            EventDefine.MVC_UI_Setting_Hide,
        };
        return notifies;
    }

    public override void HandleNotification(INotification notification)
    {
        base.HandleNotification(notification);

        switch (notification.Name)
        {
            case EventDefine.MVC_UI_Setting_FillInfo:
                this.view.FillDataInfo();
                break;
            case EventDefine.MVC_UI_Setting_Hide:
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
        if (this.view)
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
