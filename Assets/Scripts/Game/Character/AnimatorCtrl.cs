/**
 * 角色动画控制
 */

using UnityEditor.Animations;
using UnityEngine;

public class AnimatorCtrl : MonoBehaviour
{
    /// <summary>
    /// 摇杆输入向量
    /// </summary>
    private Vector3 handleInput = Vector3.zero;

    private Animator animator;

    private void Awake()
    {
        this.animator = this.GetComponent<Animator>();
        EventMgr.Instance.AddListener(EventDefine.EVE_GameControl_Move, this.PlayerMove);
    }

    private void Update()
    {
        if (this.handleInput == Vector3.zero)
        {
            this.animator.SetBool(Animator.StringToHash("Walk_Forward"), false);
            if (this.animator.GetBool("Idle") == false)
            {
                Debug.Log("=== AAA ===");
                this.animator.SetBool(Animator.StringToHash("Idle"), true);
            }
            else
            {
                Debug.Log("=== BBB ===");
            }
            return;
        }

        this.animator.SetBool(Animator.StringToHash("Idle"), false);
        this.animator.SetBool(Animator.StringToHash("Walk_Forward"), true);
    }

    private void OnDestroy()
    {
        EventMgr.Instance.RemoveListener(EventDefine.EVE_GameControl_Move, this.PlayerMove);
    }

    private void PlayerMove(IEventArgs args)
    {
        if (args == null)
        {
            return;
        }

        Vector2 temp = args.GetValue<Vector2>();
        this.handleInput = new Vector3(temp.x, 0f, temp.y);
    }

    public void Init()
    {
        if(this.animator != null)
        {
            this.animator.Play("Rifle_Idle_02");
        }
    }
}
