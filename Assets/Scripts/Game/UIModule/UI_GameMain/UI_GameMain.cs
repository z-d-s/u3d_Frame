using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameMain : BaseUI
{
	private Text		text_FrameID;
	private Button		btn_StartGame;
	private Button		btn_Setting;

	public override void Awake()
	{
		base.Awake();

        this.text_FrameID = this.transform.Find("Top_Right/text_FrameID").GetComponent<Text>();

		this.btn_StartGame = this.transform.Find("Mid_Center/btn_StartGame").GetComponent<Button>();
		this.btn_StartGame.onClick.AddListener(this.OnClickStartGame);

		this.btn_Setting = this.transform.Find("Top_Left/btn_Setting").GetComponent<Button>();
		this.btn_Setting.onClick.AddListener(this.OnClickSetting);
	}

	private void OnEnable()
	{
		this.dataProxy.RequestDataInfo();
	}

    private void OnDestroy()
    {
    }

    public override void FillDataInfo()
	{
		base.FillDataInfo();

		this.RefreshDataInfo(this.dataProxy.gameUIData);
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

	private UI_GameMain_Proxy dataProxy
	{
		get
		{
			return GameFacade.Instance.RetrieveProxy(UI_GameMain_Proxy.NAME) as UI_GameMain_Proxy;
		}
	}
}