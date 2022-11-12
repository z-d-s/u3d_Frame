using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameUI_UICtrl : UI_ctrl
{

	public override void Awake()
	{
		base.Awake();

		this.add_button_listener("attack_opt/attack_skill1", this.OnSkillClick);
	}

	private void OnSkillClick()
	{
		EventMgr.Instance.Emit("SkillAttack", null);
	}
}