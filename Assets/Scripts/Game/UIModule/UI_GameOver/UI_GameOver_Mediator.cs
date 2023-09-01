using PureMVC.Interfaces;
using PureMVC.Patterns.Mediator;
using UnityEngine;

public class UI_GameOver_Mediator : Mediator
{
    public new const string NAME = "UI_GameOver_Mediator";
    public UI_GameOver view;

    public UI_GameOver_Mediator(string mediatorName, object viewComponent = null) : base(mediatorName, viewComponent)
    {
        if(viewComponent != null)
        {
            this.view = viewComponent as UI_GameOver;
        }
    }

    public override string[] ListNotificationInterests()
    {
        string[] notifies =
        {
            EventDefine.MVC_UI_GameOver_FillInfo,
            EventDefine.MVC_UI_GameOver_Hide,
        };
        return notifies;
    }

    public override void HandleNotification(INotification notification)
    {
        base.HandleNotification(notification);

        switch(notification.Name)
        {
            case EventDefine.MVC_UI_GameOver_FillInfo:
                this.view.FillDataInfo();
                break;
            case EventDefine.MVC_UI_GameOver_Hide:
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
