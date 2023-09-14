using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_GameControl : BaseUI
{
    private Image img_Touch;
    private GameObject obj_JoyStickHandle;

    private Button btn_GameOver;

    public override void Awake()
	{
		base.Awake();

        this.img_Touch = this.transform.Find("img_Touch").GetComponent<Image>();
        this.obj_JoyStickHandle = this.transform.Find("img_Touch/JoyStickHandle").gameObject;
        this.obj_JoyStickHandle.AddComponent<JoyStickHandle>().SetTouchBg(this.img_Touch.gameObject);

        this.btn_GameOver = this.transform.Find("Top_Left/btn_GameOver").GetComponent<Button>();
        this.btn_GameOver.onClick.AddListener(this.OnClickGameOver);
    }

	void Start()
	{
	}

    private void OnClickGameOver()
    {
        GameFacade.Instance.SendNotification(EventDefine.MVC_UI_GameOver_StartUp);
    }
}
