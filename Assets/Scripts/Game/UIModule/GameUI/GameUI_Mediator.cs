using PureMVC.Interfaces;
using PureMVC.Patterns.Mediator;
using UnityEditor;
using UnityEngine;

public class GameUI_Mediator : Mediator
{
    public new const string NAME = "GameUI_Mediator";
    public GameUI view;

    public GameUI_Mediator(string mediatorName, object viewComponent = null) : base(mediatorName, viewComponent)
    {
        Debug.Log("=== AAA === GameUI_Mediator构造 ===");

        if(viewComponent != null)
        {
            this.view = viewComponent as GameUI;
        }
    }

    public override string[] ListNotificationInterests()
    {
        string[] notifies =
        {
            GameEventDefine.EV_GameUI_Change_Hp,
            GameEventDefine.EV_GameUI_Change_Exp,
            GameEventDefine.EV_GameUI_Hide,
        };
        return notifies;
    }

    public override void HandleNotification(INotification notification)
    {
        base.HandleNotification(notification);

        switch(notification.Name)
        {
            case GameEventDefine.EV_GameUI_Change_Hp:
                this.view.FillInfo(notification.Body as GameUIData);
                break;
            case GameEventDefine.EV_GameUI_Change_Exp:
                this.view.FillInfo(notification.Body as GameUIData);
                break;
            case GameEventDefine.EV_GameUI_Hide:
                this.Hide();
                break;
            default:
                break;
        }
    }

    public override void OnRegister()
    {
        base.OnRegister();

        Debug.Log("=== BBB === GameUI_Mediator::OnRegister ===");
        this.Show();
    }

    public void Show()
    {
        if(this.view)
        {
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
