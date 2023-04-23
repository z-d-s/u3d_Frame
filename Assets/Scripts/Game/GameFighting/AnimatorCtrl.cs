/**
 * 功能层组件
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorCtrl : MonoBehaviour
{
    public enum AnimState
    {
        Invalid = -1,
        Walk,
        Free,
        Idle,
        Hit,

        Attack,
        Attack2,
        Attack3,

        Skill,
        Skill2,
        Death,
    }

    private Animator anim;
    private AnimState state;
    private static string[] animNames = new string[]
    {
        "walk",
        "free",
        "idle",
        "hit",

        "attack",
        "attack2",
        "attack3",

        "skill",
        "skill2",
        "death",
    };

    public void Init()
    {
        this.anim = this.gameObject.GetComponentInChildren<Animator>();
        this.state = AnimState.Invalid;
    }

    public void SetState(AnimState state)
    {
        if(this.state == state)
        {
            return;
        }

        this.state = state;
        this.anim.Play(AnimatorCtrl.animNames[(int)this.state]);
    }
}
