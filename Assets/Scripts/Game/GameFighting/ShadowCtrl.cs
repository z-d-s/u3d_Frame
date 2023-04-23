using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowCtrl : MonoBehaviour
{
    private Transform follow = null;

    public void Init(Transform follow)
    {
        this.follow = follow;
    }

    // Update is called once per frame
    void Update()
    {
        if(this.follow != null)
        {
            this.transform.position = this.follow.position;
        }
    }
}