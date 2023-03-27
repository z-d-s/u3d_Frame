using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOpt : MonoBehaviour
{
    private CharactorCtrl ctrl;

    public void Init()
    {
        this.ctrl = this.gameObject.GetComponent<CharactorCtrl>();

        EventMgr.Instance.AddListener("SkillAttack", this.OnSkillEvent);
    }

    private void OnDestroy()
    {
        EventMgr.Instance.RemoveListener("SkillAttack", this.OnSkillEvent);
    }

    public void OnSkillEvent(IEventArgs args)
    {
        this.ctrl.OnPlayerSkill();
    }
}