using PureMVC.Patterns.Facade;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.UI;

public class GameUI : BaseUI
{
	private Image img_Blood;
	private Text text_Blood;

	private Image img_Exp;
	private Text text_Exp;

	public override void Awake()
	{
		base.Awake();

		this.img_Blood = this.view["Top_Left/user_info/img_Blood"].GetComponent<Image>();
		this.text_Blood = this.view["Top_Left/user_info/text_Blood"].GetComponent<Text>();

		this.img_Exp = this.view["Top_Left/user_info/img_Exp"].GetComponent<Image>();
		this.text_Exp = this.view["Top_Left/user_info/text_Exp"].GetComponent<Text>();

		this.AddButtonListener("Bottom_Right/attack_opt/attack_skill1", this.OnClick_Skill01);
		this.AddButtonListener("Bottom_Right/attack_opt/attack_skill2", this.OnClick_Skill02);
		this.AddButtonListener("Bottom_Right/attack_opt/attack_normall", this.OnNormalClick);

		this.AddButtonListener("Top_Right/Test_01", this.OnClickTest01);
		this.AddButtonListener("Top_Right/Test_02", this.OnClickTest02);
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

	public void RefreshDataInfo(GameUIData data)
	{
		this.img_Blood.fillAmount = data.hp / data.max_Hp;
		this.text_Blood.text = data.hp + "/" + data.max_Hp;

		this.img_Exp.fillAmount = data.exp / data.max_Exp;
		this.text_Exp.text = data.exp + "/" + data.max_Exp;
	}

	private void OnClick_Skill01()
	{
        EventMgr.Instance.Dispatch("SkillAttack");

        AssetsLoadMgr.Instance.LoadAsync("effect_asset", "Effects/Prefabs/swords.prefab", (string name, UnityEngine.Object obj) =>
        {
			GameObject o = PoolMgr.Instance.GetObject(obj.name, obj as GameObject);
			o.transform.position = GameMgr.Instance.player.transform.position;
			TimeMgr.Instance.DoOnce(1000, () =>
			{
				PoolMgr.Instance.RecycleObject(o.name, o);
			});
        });
    }

	private void OnClick_Skill02()
	{
        AssetsLoadMgr.Instance.LoadAsync("effect_asset", "Effects/Prefabs/landcuts.prefab", (string name, UnityEngine.Object obj) =>
        {
			GameObject o = PoolMgr.Instance.GetObject(obj.name, obj as GameObject);
            o.transform.position = GameMgr.Instance.player.transform.position;
            TimeMgr.Instance.DoOnce(600, () =>
			{
				PoolMgr.Instance.RecycleObject(o.name, o);
			});
        });
    }

    private void OnNormalClick()
	{
		this.Hide();
	}

    private void OnClickTest01()
	{
        TimeMgr.Instance.DoLoop(1000, this.Test);
    }

	private void OnClickTest02()
	{
		TimeMgr.Instance.ClearTime(this.Test);
	}

	private void Test()
	{
        UtilLog.Log("测试方法");
    }

    private GameUI_Proxy dataProxy
    {
        get
        {
            return AppFacade.Instance.RetrieveProxy(GameUI_Proxy.NAME) as GameUI_Proxy;
        }
    }
}