using PureMVC.Interfaces;
using PureMVC.Patterns.Mediator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Loading_Mediator : Mediator
{
    public new const string NAME = "UI_Loading_Mediator";
    public UI_Loading view;

    public UI_Loading_Mediator(string mediatorName, object viewComponent = null) : base(mediatorName, viewComponent)
    {
        if (viewComponent != null)
        {
            this.view = viewComponent as UI_Loading;
        }
    }

    public override string[] ListNotificationInterests()
    {
        string[] notifies =
        {
            EventDefine.MVC_UI_Loading_FillInfo,
            EventDefine.MVC_UI_Loading_Hide,
        };
        return notifies;
    }

    public override void HandleNotification(INotification notification)
    {
        base.HandleNotification(notification);

        switch (notification.Name)
        {
            case EventDefine.MVC_UI_Loading_FillInfo:
                this.view.FillDataInfo();
                break;
            case EventDefine.MVC_UI_Game_Hide:
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
