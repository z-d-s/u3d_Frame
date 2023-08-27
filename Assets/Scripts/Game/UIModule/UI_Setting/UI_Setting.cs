using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_Setting : BaseUI
{
	private Button		btn_Mask;
	private Button		btn_Language_CN;
	private Button		btn_Language_EN;

	public override void Awake()
	{
		base.Awake();

		this.btn_Mask = this.transform.Find("btn_Mask").GetComponent<Button>();
		this.btn_Language_CN = this.transform.Find("btn_Language_CN").GetComponent<Button>();
		this.btn_Language_EN = this.transform.Find("btn_Language_EN").GetComponent<Button>();

		this.btn_Mask.onClick.AddListener(this.OnClickMask);
		this.btn_Language_CN.onClick.AddListener(this.OnClickLanguageCN);
		this.btn_Language_EN.onClick.AddListener(this.OnClickLanguageEN);
	}

	void Start()
	{
	}

	private void OnClickMask()
	{
		this.gameObject.SetActive(false);
	}

	private void OnClickLanguageCN()
	{
		LanguageMgr.Instance.SetLanguage(LanguageType.Chinese);
	}

	private void OnClickLanguageEN()
	{
        LanguageMgr.Instance.SetLanguage(LanguageType.English);
	}
}
