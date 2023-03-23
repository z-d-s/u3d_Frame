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
            GameEventDefine.GAMEUI_CHANGE_COIN,
            GameEventDefine.GAMEUI_CHANGE_SCORE,
            GameEventDefine.GAMEUI_CHANGE_ENERGY,
            GameEventDefine.GAMEUI_HIDE,
        };
        return notifies;
    }

    public override void HandleNotification(INotification notification)
    {
        base.HandleNotification(notification);

        switch(notification.Name)
        {
            case GameEventDefine.GAMEUI_CHANGE_COIN:
                break;
            case GameEventDefine.GAMEUI_CHANGE_SCORE:
                break;
            case GameEventDefine.GAMEUI_CHANGE_ENERGY:
                break;
            case GameEventDefine.GAMEUI_HIDE:
                if (this.view)
                {
                    this.view.Hide();
                }
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
