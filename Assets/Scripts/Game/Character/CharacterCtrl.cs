/**
 * 角色逻辑控制
 */

using UnityEditor.Animations;
using UnityEngine;

public class CharacterCtrl : MonoBehaviour
{
    public bool shooting = false;

    private void Awake()
    {
        EventMgr.Instance.AddListener(EventDefine.EVE_StartShooting, this.StartShooting);
        EventMgr.Instance.AddListener(EventDefine.EVE_StopShooting, this.StopShooting);
    }

    private void Update()
    {
        
    }

    private void OnDestroy()
    {
        EventMgr.Instance.RemoveListener(EventDefine.EVE_StartShooting, this.StartShooting);
        EventMgr.Instance.RemoveListener(EventDefine.EVE_StopShooting, this.StopShooting);
    }

    private void StartShooting(IEventArgs args)
    {
        this.shooting = true;
    }

    private void StopShooting(IEventArgs args)
    {
        this.shooting = false;
    }
}
