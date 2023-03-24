using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIData
{
    public float hp = 0;
    public float max_Hp = 0;

    public float exp = 0;
    public float max_Exp = 0;

    public GameUIData(float _max_Hp, float _max_Exp)
    {
        this.hp = _max_Hp;
        this.max_Hp = _max_Hp;

        this.exp = _max_Exp;
        this.max_Exp = _max_Exp;
    }
}
