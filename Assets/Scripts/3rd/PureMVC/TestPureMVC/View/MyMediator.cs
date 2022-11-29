using PureMVC.Interfaces;
using PureMVC.Patterns.Mediator;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MyMediator : Mediator
{
    public const string mediatorName = "MyMediator_Name";
    private TextMeshProUGUI txt_Number;
    private Button btn_Add;
    private Button btn_Sub;

    public MyMediator(string mediatorName, GameObject viewComponent = null) : base(mediatorName, viewComponent)
    {
        this.txt_Number = viewComponent.transform.Find("txt_Number").GetComponent<TextMeshProUGUI>();
        this.btn_Add = viewComponent.transform.Find("btn_Add").GetComponent<Button>();
        this.btn_Sub = viewComponent.transform.Find("btn_Sub").GetComponent<Button>();

        this.btn_Add.onClick.AddListener(this.OnClickBtnAdd);
        this.btn_Sub.onClick.AddListener(this.OnClickBtnSub);
    }

    /// <summary>
    /// 接收哪些消息
    /// </summary>
    /// <returns></returns>
    public override string[] ListNotificationInterests()
    {
        string[] list_Msg = new string[2];
        list_Msg[0] = "msg_add_s";
        list_Msg[1] = "msg_sub_s";
        return list_Msg;
    }

    /// <summary>
    /// 得到消息后执行什么
    /// </summary>
    /// <param name="notification"></param>
    public override void HandleNotification(INotification notification)
    {
        base.HandleNotification(notification);
        switch(notification.Name)
        {
            case "msg_add_s":
                this.ShowNumber(notification.Body as MyData);
                break;
            case "msg_sub_s":
                this.ShowNumber(notification.Body as MyData);
                break;
            default:
                break;
        }
    }

    private void ShowNumber(MyData data)
    {
        Debug.Log("===" + data.dataValue.ToString() + "===");
        this.txt_Number.text = data.dataValue.ToString();
    }

    private void OnClickBtnAdd()
    {
        SendNotification("msg_add_f");
    }

    private void OnClickBtnSub()
    {
        SendNotification("msg_sub_f");
    }
}
