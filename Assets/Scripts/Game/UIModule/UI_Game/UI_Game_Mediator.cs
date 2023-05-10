using PureMVC.Interfaces;
using PureMVC.Patterns.Mediator;
using System;
using System.Xml.Linq;
using UnityEditor;
using UnityEngine;

public class UI_Game_Mediator : Mediator
{
    public new const string NAME = "UI_Game_Mediator";
    public UI_Game view;

    public UI_Game_Mediator(string mediatorName, object viewComponent = null) : base(mediatorName, viewComponent)
    {
        if(viewComponent != null)
        {
            this.view = viewComponent as UI_Game;
        }
    }

    public override string[] ListNotificationInterests()
    {
        string[] notifies =
        {
            EventDefine.MVC_UI_Game_FillInfo,
            EventDefine.MVC_UI_Game_Change_Hp,
            EventDefine.MVC_UI_Game_Change_Score,
            EventDefine.MVC_UI_Game_RefreshTouchingTime,
            EventDefine.MVC_UI_Game_Hide,
        };
        return notifies;
    }

    public override void HandleNotification(INotification notification)
    {
        base.HandleNotification(notification);

        switch(notification.Name)
        {
            case EventDefine.MVC_UI_Game_FillInfo:
                this.view.FillDataInfo();
                break;
            case EventDefine.MVC_UI_Game_Change_Hp:
                this.view.RefreshDataInfo(notification.Body as UI_GameData);
                break;
            case EventDefine.MVC_UI_Game_Change_Score:
                this.view.RefreshDataInfo(notification.Body as UI_GameData);
                break;
            case EventDefine.MVC_UI_Game_RefreshTouchingTime:
                this.view.RefreshTouchingTime((float)notification.Body);
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
