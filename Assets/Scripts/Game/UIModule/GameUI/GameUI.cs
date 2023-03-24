using Unity.VisualScripting;
using UnityEngine;
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

		this.img_Blood = this.view["user_info/img_Blood"].GetComponent<Image>();
		this.text_Blood = this.view["user_info/text_Blood"].GetComponent<Text>();

		this.img_Exp = this.view["user_info/img_Exp"].GetComponent<Image>();
		this.text_Exp = this.view["user_info/text_Exp"].GetComponent<Text>();

		this.AddButtonListener("attack_opt/attack_skill1", this.OnSkillClick);
	}

	private void OnEnable()
	{
		
	}

	public void FillInfo(GameUIData data)
	{
		this.img_Blood.fillAmount = data.hp / data.max_Hp;
		this.text_Blood.text = data.hp + "/" + data.max_Hp;

		this.img_Exp.fillAmount = data.exp / data.max_Exp;
		this.text_Exp.text = data.exp + "/" + data.max_Exp;
	}

	private void OnSkillClick()
	{
        //EventMgr.Instance.Emit("SkillAttack", null);

        AssetsLoadMgr.Instance.LoadAsync("effect_asset", "Effects/Prefabs/swords.prefab", (string name, UnityEngine.Object obj) =>
        {
            GameObject charactorPrefab = obj as GameObject;
            GameObject e = GameObject.Instantiate(charactorPrefab);
            e.transform.position = GameMgr.Instance.player.transform.position + new Vector3(1, 0, 1);
            e.name = "sword";
        });
    }
}