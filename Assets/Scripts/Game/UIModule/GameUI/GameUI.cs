using Unity.VisualScripting;

public class GameUI : BaseUI
{
	public override void Awake()
	{
		base.Awake();

		this.AddButtonListener("attack_opt/attack_skill1", this.OnSkillClick);
	}

	private void OnSkillClick()
	{
		EventMgr.Instance.Emit("SkillAttack", null);
	}
}