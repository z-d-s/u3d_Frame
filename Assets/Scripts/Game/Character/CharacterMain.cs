using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CharacterMain : CharacterBase
{
    /// <summary>
    /// 攻击目标
    /// </summary>
    private CharacterBase attackTarget;

    /// <summary>
    /// 摇杆输入向量
    /// </summary>
    private Vector3 handleInput = Vector3.zero;

    /// <summary>
    /// 武器的挂点
    /// </summary>
    private Transform hangPoint;

    /// <summary>
    /// 角色使用中的武器
    /// </summary>
    private Weapon_Gun mainGun = null;

    private CharacterCtrl characterCtrl = null;

    private void Awake()
    {
        this.characterCtrl = this.AddComponent<CharacterCtrl>();

        EventMgr.Instance.AddListener(EventDefine.EVE_GameControl_Move, this.PlayerMove);
        EventMgr.Instance.AddListener(EventDefine.EVE_GameRestart, this.GameRestart);
    }

    private void Update()
    {
        this.UpdateHangWeapon();
        this.UpdateSelfRot();
        this.UpdateSelfPos();
        this.UpdateShoot();

        //调试射线
        this.UpdateDrawLine();
    }

    private void UpdateHangWeapon()
    {
        if (this.real_Character == null)
        {
            return;
        }

        if(this.hangPoint != null)
        {
            return;
        }

        this.hangPoint = this.real_Character.transform.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/mixamorig:RightForeArm/mixamorig:RightHand/Gun_HangPoint");

        string weaponID = "4102";
        int weaponLevel = 5;
        EquipmentMgr.Instance.CreateWeapon(weaponID, (Weapon_Gun gun) =>
        {
            if(gun ==  null)
            {
                return;
            }

            this.mainGun = gun;
            this.mainGun.Init(this, this.hangPoint, weaponLevel);
        });
    }

    private void UpdateSelfRot()
    {
        if (teamType != Enum_TeamType.Self)
        {
            return;
        }

        if(this.real_Character ==  null)
        {
            return;
        }

        if(this.attackTarget == null)
        {
            if(this.circleMarkCtrl !=  null)
            {
                //Vector3.MoveTowards(this.real_Character.transform.forward, this.circleMarkCtrl.transform.forward, 0f);
                this.real_Character.transform.rotation = Quaternion.Slerp(this.real_Character.transform.rotation, this.circleMarkCtrl.transform.rotation, this.characterBaseData.rotateSpeed * Time.deltaTime);
            }
        }
        else
        {
            this.transform.LookAt(this.attackTarget.transform.position);
            Vector3 targetDir = this.attackTarget.transform.position - this.real_Character.transform.position;
            Vector3 newDir = Vector3.RotateTowards(this.real_Character.transform.forward, targetDir, this.characterBaseData.rotateSpeed * Time.deltaTime, 0f);
            this.real_Character.transform.rotation = Quaternion.LookRotation(newDir);
        }
    }

    private void UpdateSelfPos()
    {
        if(teamType != Enum_TeamType.Self)
        {
            return;
        }

        if (this.handleInput == Vector3.zero)
        {
            return;
        }

        this.transform.Translate(this.handleInput * this.characterBaseData.moveSpeed, Space.World);
    }

    float cdTime = 100f;
    private void UpdateShoot()
    {
        if(this.characterCtrl.shooting == false)
        {
            return;
        }

        if(this.mainGun == null)
        {
            return;
        }

        this.cdTime += Time.deltaTime;
        if(this.cdTime >= this.mainGun.weaponData.attackSpeed)
        {
            this.cdTime = 0f;

            EquipmentMgr.Instance.CreateBullet(this.mainGun.weaponData.shootingBulletID, (ShootingBullet bullet) =>
            {
                if(bullet ==  null)
                {
                    return;
                }

                Debug.Log(this.real_Character.transform.forward);
                bullet.Init(this, this.mainGun.GetBulletSpawnPoint(), this.real_Character.transform.forward);
            });
        }
    }

    private Ray ray;
    private void UpdateDrawLine()
    {
        if(this.mainGun == null)
        {
            return;
        }

        if(this.ray.IsUnityNull())
        {
            this.ray = new Ray();
        }

        ray.origin = this.GetCharacterPos() + new Vector3(0f, 1f, 0f);
        ray.direction = this.GetCharacterForward();
        Debug.DrawRay(ray.origin, ray.direction * this.mainGun.weaponData.attackDistance, Color.green);
    }

    private void OnDestroy()
    {
        EventMgr.Instance.RemoveListener(EventDefine.EVE_GameControl_Move, this.PlayerMove);
        EventMgr.Instance.RemoveListener(EventDefine.EVE_GameRestart, this.GameRestart);
    }

    public void Init(int level, Enum_TeamType type, Vector3 pos)
    {
        this.isDeath = false;
        this.teamType = type;

        this.SetBaseControl();
        this.RefreshDataByLevel(level);
        this.SetCharacterPos(pos);
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

    public void SetTarget(CharacterBase target = null)
    {
        if(target == null)
        {
            EventMgr.Instance.Dispatch(EventDefine.EVE_StopShooting);
        }
        else
        {
            this.attackTarget = target;
            EventMgr.Instance.Dispatch(EventDefine.EVE_StartShooting);
        }
    }

    public CharacterBase GetTarget()
    {
        return this.attackTarget;
    }

    public void RefreshDataByLevel(int level = 1)
    {
        this.isDeath = false;

        Tbl_Character_Level tbl_Character_Level = TableMgr.Instance.GetTable<Tbl_Character_Level>(UtilTool.GetIDByIDAndLevel(this.characterBaseData.id, level));
        if (tbl_Character_Level == null)
        {
            tbl_Character_Level = TableMgr.Instance.GetTable<Tbl_Character_Level>(UtilTool.GetIDByIDAndLevel(this.characterBaseData.id, 1));
        }

        this.characterBaseData.moveSpeed = tbl_Character_Level.MoveSpeed;
        this.characterBaseData.rotateSpeed = tbl_Character_Level.RotateSpeed;
        this.characterBaseData.hp = tbl_Character_Level.Hp;
    }
}
