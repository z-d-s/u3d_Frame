using UnityEngine;

public class CharacterEnemy : CharacterBase
{
    /// <summary>
    /// 摇杆输入向量
    /// </summary>
    private Vector3 handleInput = Vector3.zero;

    private void Awake()
    {
        EventMgr.Instance.AddListener(EventDefine.EVE_GameRestart, this.GameRestart);
    }

    private void Update()
    {
        if (this.handleInput == Vector3.zero)
        {
            return;
        }

        this.transform.Translate(this.handleInput * this.characterBaseData.moveSpeed, Space.World);
        this.transform.localRotation = Quaternion.Euler(new Vector3(0f, Vector3.SignedAngle(Vector3.forward, this.handleInput, Vector3.up), 0f));
    }

    private void OnDestroy()
    {
        EventMgr.Instance.RemoveListener(EventDefine.EVE_GameRestart, this.GameRestart);
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

    private void GameRestart(IEventArgs args)
    {
        
    }

    public void RefreshDataByLevel(int level = 1)
    {
        Tbl_Enemy_Level tbl_Enemy_Level = TableMgr.Instance.GetTable<Tbl_Enemy_Level>(level.ToString());
        if (tbl_Enemy_Level == null)
        {
            tbl_Enemy_Level = TableMgr.Instance.GetTable<Tbl_Enemy_Level>("1");
        }

        this.characterBaseData.moveSpeed = tbl_Enemy_Level.MoveSpeed;
        this.characterBaseData.rotateSpeed = tbl_Enemy_Level.RotateSpeed;
        this.characterBaseData.hp = tbl_Enemy_Level.Hp;
    }
}
