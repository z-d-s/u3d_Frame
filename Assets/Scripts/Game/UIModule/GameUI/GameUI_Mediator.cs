using PureMVC.Interfaces;
using PureMVC.Patterns.Mediator;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI_Mediator : Mediator
{
    public new const string NAME = "GameUI_Mediator";
    public GameUI view;

    public GameUI_Mediator(string mediatorName, object viewComponent = null) : base(mediatorName, viewComponent)
    {
        Debug.Log("=== AAA === GameUI_Mediator构造 ===");
    }

    public override string[] ListNotificationInterests()
    {
        string[] notifies =
        {
            EnumGameNotify.CHANGE_COIN.ToString(),
            EnumGameNotify.CHANGE_SCORE.ToString(),
            EnumGameNotify.CHANGE_ENERGY.ToString(),
        };
        return notifies;
    }

    public override void HandleNotification(INotification notification)
    {
        base.HandleNotification(notification);

        if(EnumGameNotify.CHANGE_COIN.ToString() == notification.Name)
        {

        }
        else if(EnumGameNotify.CHANGE_SCORE.ToString() == notification.Name)
        {

        }
        else if(EnumGameNotify.CHANGE_ENERGY.ToString() == notification.Name)
        {

        }
    }

    public override void OnRegister()
    {
        base.OnRegister();

        Debug.Log("=== BBB === GameUI_Mediator::OnRegister ===");

        string path = "GUI/UIPrefabs/GameUI.prefab";
        GameObject ui_prefab = (GameObject)AssetsLoadMgr.Instance.LoadSync("ui_gameui", path);
        GameObject ui_view = GameObject.Instantiate(ui_prefab);
        ui_view.name = ui_prefab.name;
        ui_view.transform.SetParent(GameObject.Find("Canvas").transform, false);
        this.view = ui_view.AddComponent<GameUI>();
    }
}
