public class GameUI : UI_Base
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