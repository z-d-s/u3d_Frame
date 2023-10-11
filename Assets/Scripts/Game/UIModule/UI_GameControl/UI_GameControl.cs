using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;

public class UI_GameControl : BaseUI
{
    private Image img_Touch;
    private JoyStickHandle joyHandle;
    private KeyCodeHandle keyCodeHandle;

    private Button btn_GameOver;

    public override void Awake()
	{
		base.Awake();

        this.img_Touch = this.transform.Find("img_Touch").GetComponent<Image>();
        this.joyHandle = this.transform.Find("img_Touch/JoyStickHandle").AddComponent<JoyStickHandle>();
        this.joyHandle.SetTouchBg(this.img_Touch.gameObject);

        this.keyCodeHandle = new KeyCodeHandle();
        this.keyCodeHandle.SetJoyHandle(this.joyHandle);

        this.btn_GameOver = this.transform.Find("Top_Left/btn_GameOver").GetComponent<Button>();
        this.btn_GameOver.onClick.AddListener(this.OnClickGameOver);
    }

    public override void OnEnable()
    {
        this.dataProxy.RequestDataInfo();
    }

    private void Start()
	{
	}

    private void Update()
    {
        this.keyCodeHandle?.Update();
    }

    public override void FillDataInfo()
    {
        base.FillDataInfo();
    }

    private void OnClickGameOver()
    {
        GameFacade.Instance.SendNotification(EventDefine.MVC_UI_GameOver_StartUp);
    }

    private UI_GameOver_Proxy dataProxy
    {
        get
        {
            return GameFacade.Instance.RetrieveProxy(UI_GameOver_Proxy.NAME) as UI_GameOver_Proxy;
        }
    }
}
