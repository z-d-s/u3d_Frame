using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameOver : BaseUI
{
	private Text		text_GameOver;
	private Button		btn_RestartGame;

	public override void Awake()
	{
		base.Awake();

		this.text_GameOver = this.transform.Find("Mid_Center/text_GameOver").GetComponent<Text>();
		this.btn_RestartGame = this.transform.Find("Mid_Center/btn_RestartGame").GetComponent<Button>();
		this.btn_RestartGame.onClick.AddListener(this.OnClickRestartGame);
	}

	private void OnEnable()
	{
		this.dataProxy.RequestDataInfo();
	}

	public override void FillDataInfo()
	{
		base.FillDataInfo();

		this.RefreshDataInfo(this.dataProxy.gameUIData);
	}

	public void RefreshDataInfo(UI_GameData data)
	{
		
	}

	public void RefreshTouchingTime(float time)
	{
		this.text_GameOver.text = $"TouchingTime:{string.Format("{0:F2}", time)}";
	}

	private void OnClickRestartGame()
	{
		LogHelper.Log("=== 点击重新开始游戏 ===");

		this.Hide();
		EventMgr.Instance.Dispatch(EventDefine.EVE_GameRestart);
	}

	private UI_GameOver_Proxy dataProxy
	{
		get
		{
			return AppFacade.Instance.RetrieveProxy(UI_GameOver_Proxy.NAME) as UI_GameOver_Proxy;
		}
	}
}