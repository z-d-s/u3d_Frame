using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    private SmoothFollow smoothFollow = null;
    private Vector3 followPos = new Vector3(0f, 20f, -7f);
    private float smoothRatio = 1f;

    private Vector3 followAngle = new Vector3(70, 0, 0);

    public void Init()
    {
        this.transform.eulerAngles = this.followAngle;

        this.smoothFollow = this.transform.AddComponent<SmoothFollow>();
        this.smoothFollow.SetTarget(GameMgr.Instance.characterMain.gameObject, followPos, this.smoothRatio);
    }
}
