using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_Loading : BaseUI
{
    private bool loading = false;
    private float currentProgress = 0f;
    private float speed = 0.6f;
    private Image img_LoadingBar;

    public override void Awake()
    {
	    base.Awake();
        this.img_LoadingBar = this.transform.Find("Loading/LoadingBar").GetComponent<Image>();
    }

    private void OnEnable()
    {
	    this.currentProgress = 0f;
        this.loading = true;
    }

    private void Update()
    {
        if(this.loading == false)
        {
            return;
        }

        if(this.img_LoadingBar == null)
        {
            return;
        }

        if(this.currentProgress >= 1f)
        {
            this.loading = false;
            this.SetLoadingBar(1f);

            AppFacade.Instance.SendNotification(EventDefine.MVC_UI_Loading_Hide);
            GameApp.Instance.EnterMainScene();
            return;
        }

        this.currentProgress += speed * Time.deltaTime;
        this.SetLoadingBar(this.currentProgress);
    }

    private void SetLoadingBar(float progress)
    {
        this.img_LoadingBar.fillAmount = progress;
    }
}
