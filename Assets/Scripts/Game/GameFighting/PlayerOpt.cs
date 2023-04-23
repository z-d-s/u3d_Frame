/**
 * 自身操控角色(实现屏幕前玩家的主动操控响应)
 */

using UnityEngine;

public class PlayerOpt : MonoBehaviour
{
    private CharacterBase ctrl;

    public void Init()
    {
        this.ctrl = this.gameObject.GetComponent<CharacterBase>();

        EventMgr.Instance.AddListener("SkillAttack", this.OnSkillEvent);
    }

    private void OnDestroy()
    {
        EventMgr.Instance.RemoveListener("SkillAttack", this.OnSkillEvent);
    }

    public void OnSkillEvent(IEventArgs args)
    {
        
    }
}