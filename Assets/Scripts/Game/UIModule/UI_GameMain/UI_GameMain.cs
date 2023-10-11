using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameMain : BaseUI
{
	private Button		btn_StartGame;
	private Button		btn_Setting;
	private Button		btn_Announcement;

	public override void Awake()
	{
		base.Awake();

		this.btn_StartGame = this.transform.Find("Mid_Center/btn_StartGame").GetComponent<Button>();
		this.btn_StartGame.onClick.AddListener(this.OnClickStartGame);

		this.btn_Setting = this.transform.Find("Top_Left/btn_Setting").GetComponent<Button>();
		this.btn_Setting.onClick.AddListener(this.OnClickSetting);

		this.btn_Announcement = this.transform.Find("Top_Left/btn_Announcement").GetComponent<Button>();
		this.btn_Announcement.onClick.AddListener(this.OnClickAnnouncement);
	}

	public override void OnEnable()
	{
		this.dataProxy.RequestDataInfo();
	}

    private void OnDestroy()
    {
    }

    public override void FillDataInfo()
	{
		base.FillDataInfo();

		this.RefreshDataInfo(this.dataProxy.gameMainData);
	}

	public void RefreshDataInfo(UI_GameMainData data)
	{
		
	}

	private void OnClickStartGame()
	{
		LogHelper.Log("=== 点击开始游戏 ===");
		GameMgr.Instance.EnterFightingScene();
	}

	private void OnClickSetting()
	{
		GameFacade.Instance.SendNotification(EventDefine.MVC_UI_Setting_StartUp);
	}

	private void OnClickAnnouncement()
	{
		GameFacade.Instance.SendNotification(EventDefine.MVC_UI_Announcement_StartUp);
	}

	private UI_GameMain_Proxy dataProxy
	{
		get
		{
			return GameFacade.Instance.RetrieveProxy(UI_GameMain_Proxy.NAME) as UI_GameMain_Proxy;
		}
	}
}