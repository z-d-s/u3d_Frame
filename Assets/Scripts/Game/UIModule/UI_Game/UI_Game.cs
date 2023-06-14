using System;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class UI_Game : BaseUI
{
	private Image		img_Touch;
	private Text		text_FrameID;
	private Text		text_TouchingTime;
	private Button		btn_StartGame;

	public override void Awake()
	{
		base.Awake();

		this.img_Touch = this.transform.Find("img_Touch").GetComponent<Image>();
		this.img_Touch.AddComponent<GameTouchCtrl>();

		this.text_TouchingTime = this.transform.Find("Top_Right/text_FrameID").GetComponent<Text>();
		this.text_TouchingTime = this.transform.Find("Top_Right/text_TouchingTime").GetComponent<Text>();

		this.btn_StartGame = this.transform.Find("Mid_Center/btn_StartGame").GetComponent<Button>();
		this.btn_StartGame.onClick.AddListener(this.OnClickStartGame);

		EventMgr.Instance.AddListener(EventDefine.EVE_GameRestart, this.GameRestart);
	}

	private void OnEnable()
	{
		this.SetStartBtnActive(true);
		this.dataProxy.RequestDataInfo();
	}

    private void OnDestroy()
    {
		EventMgr.Instance.RemoveListener(EventDefine.EVE_GameRestart, this.GameRestart);
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
		this.text_TouchingTime.text = $"TouchingTime:{string.Format("{0:F2}", time)}";
	}

	private void OnClickStartGame()
	{
		LogHelper.Log("=== 点击开始游戏 ===");

		this.SetStartBtnActive(false);
		GameMgr.Instance.gameState = GameState.Gameing;

		LogHelper.LogBlue("需要新的基座02");
		GameObject foundationObj = PoolMgr.Instance.GetObject("map_foundation", "Maps/Prefabs/Foundation_001.prefab");
		FoundationNode node = new FoundationNode();
		node.nodeObj = foundationObj;
		GameMgr.Instance.characterMain.currentFoundationNode.SetNextNode(node);
		node.SetOffsetPos(new Vector3(0, 0, 6));
		node.Init(2f, 2f);
	}

	private void SetStartBtnActive(bool active)
	{
		this.btn_StartGame.gameObject.SetActive(active);
	}

	private void GameRestart(IEventArgs args)
	{
		this.OnClickStartGame();
	}

	private UI_Game_Proxy dataProxy
	{
		get
		{
			return AppFacade.Instance.RetrieveProxy(UI_Game_Proxy.NAME) as UI_Game_Proxy;
		}
	}
}