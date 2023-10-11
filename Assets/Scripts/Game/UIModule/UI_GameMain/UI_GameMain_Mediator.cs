using PureMVC.Interfaces;
using PureMVC.Patterns.Mediator;
using System;
using System.Xml.Linq;
using UnityEditor;
using UnityEngine;

public class UI_GameMain_Mediator : Mediator
{
    public new const string NAME = "UI_GameMain_Mediator";
    public UI_GameMain view;

    public UI_GameMain_Mediator(string mediatorName, object viewComponent = null) : base(mediatorName, viewComponent)
    {
        if(viewComponent != null)
        {
            this.view = viewComponent as UI_GameMain;
        }
    }

    public override string[] ListNotificationInterests()
    {
        string[] notifies =
        {
            EventDefine.MVC_UI_GameMain_FillInfo,
            EventDefine.MVC_UI_GameMain_Hide,
            EventDefine.MVC_UI_GameMain_Change_Coin,
            EventDefine.MVC_UI_GameMain_Change_Diamond,
        };
        return notifies;
    }

    public override void HandleNotification(INotification notification)
    {
        base.HandleNotification(notification);

        switch(notification.Name)
        {
            case EventDefine.MVC_UI_GameMain_FillInfo:
                this.view.FillDataInfo();
                break;
            case EventDefine.MVC_UI_GameMain_Hide:
                this.Hide();
                break;
            case EventDefine.MVC_UI_GameMain_Change_Coin:
                this.view.RefreshDataInfo(notification.Body as UI_GameMainData);
                break;
            case EventDefine.MVC_UI_GameMain_Change_Diamond:
                this.view.RefreshDataInfo(notification.Body as UI_GameMainData);
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
