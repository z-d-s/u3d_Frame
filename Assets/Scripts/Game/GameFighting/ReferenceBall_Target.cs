using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceBall_Target : MonoBehaviour
{
    private void Awake()
    {
        this.gameObject.transform.localScale = Vector3.one * 0.3f;
    }

    private void OnDestroy()
    {
    }

    public void SetPos(Vector3 pos)
    {
        this.transform.position = pos;
    }
}