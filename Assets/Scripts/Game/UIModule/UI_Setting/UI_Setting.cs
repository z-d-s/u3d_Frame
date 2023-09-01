using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_Setting : BaseUI
{
	private Button		btn_Mask;
	private Button		btn_Language_CN;
	private Button		btn_Language_EN;
	private Text text_01;
	private Text text_02;
	private Text text_03;

	public override void Awake()
	{
		base.Awake();

		this.btn_Mask = this.transform.Find("btn_Mask").GetComponent<Button>();
		this.btn_Language_CN = this.transform.Find("btn_Language_CN").GetComponent<Button>();
		this.btn_Language_EN = this.transform.Find("btn_Language_EN").GetComponent<Button>();

		this.btn_Mask.onClick.AddListener(this.OnClickMask);
		this.btn_Language_CN.onClick.AddListener(this.OnClickLanguageCN);
		this.btn_Language_EN.onClick.AddListener(this.OnClickLanguageEN);

		this.text_01 = this.transform.Find("text_01").GetComponent<Text>();
		this.text_02 = this.transform.Find("text_02").GetComponent<Text>();
		this.text_03 = this.transform.Find("text_03").GetComponent<Text>();
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
		LanguageData.SetLanguage(LanguageType.Chinese);
	}

	private void OnClickLanguageEN()
	{
        LanguageData.SetLanguage(LanguageType.English);
	}
}
