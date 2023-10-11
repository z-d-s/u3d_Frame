using UnityEngine;

public class CircleMarkCtrl : MonoBehaviour
{
    /// <summary>
    /// 跟随的角色
    /// </summary>
    private CharacterBase target = null;

    /// <summary>
    /// 摇杆输入向量
    /// </summary>
    private Vector3 handleInput = Vector3.zero;

    private void Awake()
    {
        EventMgr.Instance.AddListener(EventDefine.EVE_GameControl_Move, this.PlayerMove);
    }

    private void Update()
    {
        this.UpdateSelfPos();
        this.UpdateSelfRot();
    }

    private void UpdateSelfPos()
    {
        if (this.target == null)
        {
            return;
        }

        this.transform.position = this.target.transform.position + new Vector3(0f, 0.02f, 0f);
    }

    private void UpdateSelfRot()
    {
        if (this.handleInput == Vector3.zero)
        {
            return;
        }

        this.transform.localRotation = Quaternion.Euler(new Vector3(0f, Vector3.SignedAngle(Vector3.forward, this.handleInput, Vector3.up), 0f));
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

    public void SetTarget(CharacterBase _target)
    {
        this.target = _target;
    }

    public Vector3 GetLocalEulerAngle()
    {
        return this.transform.localEulerAngles;
    }
}