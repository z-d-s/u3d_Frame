using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BaseUI : MonoBehaviour
{
    public virtual void Awake()
    {
    }

    public virtual void OnEnable()
    {
        
    }

    public virtual void OnDisable()
    {
        
    }

    public virtual void FillDataInfo()
    {
    }

    public virtual void Show()
    {
        this.gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
