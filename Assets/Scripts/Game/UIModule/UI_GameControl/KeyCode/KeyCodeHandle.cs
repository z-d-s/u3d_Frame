using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCodeHandle
{
    private JoyStickHandle joyHandle;

    private bool up_Valide = false;
    private bool left_Valide = false;
    private bool down_Valide = false;
    private bool right_Valide = false;

    public void SetJoyHandle(JoyStickHandle _joyHandle)
    {
        this.joyHandle = _joyHandle;
    }
    public void Update()
    {
        if (this.joyHandle == null)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            this.up_Valide = true;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            this.up_Valide = false;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            this.left_Valide = true;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            this.left_Valide = false;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            this.down_Valide = true;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            this.down_Valide = false;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            this.right_Valide = true;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            this.right_Valide = false;
        }


        if (this.up_Valide)
        {
            if (this.left_Valide)
            {
                this.joyHandle.SetByAngle(new Vector2(-1, 1));
            }
            else if (this.right_Valide)
            {
                this.joyHandle.SetByAngle(new Vector2(1, 1));
            }
            else
            {
                this.joyHandle.SetByAngle(Vector2.up);
            }
        }
        else if (this.down_Valide)
        {
            if (this.left_Valide)
            {
                this.joyHandle.SetByAngle(new Vector2(-1, -1));
            }
            else if (this.right_Valide)
            {
                this.joyHandle.SetByAngle(new Vector2(1, -1));
            }
            else
            {
                this.joyHandle.SetByAngle(Vector2.down);
            }
        }
        else if (this.left_Valide)
        {
            this.joyHandle.SetByAngle(Vector2.left);
        }
        else if (this.right_Valide)
        {
            this.joyHandle.SetByAngle(Vector2.right);
        }
        else
        {
            this.joyHandle.SetByAngle(Vector2.zero);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            EventMgr.Instance.Dispatch(EventDefine.EVE_StartShooting);
        }
        if(Input.GetKeyUp(KeyCode.Space))
        {
            EventMgr.Instance.Dispatch(EventDefine.EVE_StopShooting);
        }
    }
}
