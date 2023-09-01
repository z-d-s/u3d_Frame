using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_Setting : BaseUI
{
	private Button		btn_Mask;
	private Button		btn_Language_CN;
	private Button		btn_Language_EN;
	private Button		btn_Language_JP;
	private InputField	input_LanguageID;
	private Button		btn_Go;
	private Text		text_Welcome;
	private Text		text_Show;

	public override void Awake()
	{
		base.Awake();

		this.btn_Mask = this.transform.Find("btn_Mask").GetComponent<Button>();
		this.btn_Language_CN = this.transform.Find("btn_Language_CN").GetComponent<Button>();
		this.btn_Language_EN = this.transform.Find("btn_Language_EN").GetComponent<Button>();
		this.btn_Language_JP = this.transform.Find("btn_Language_JP").GetComponent<Button>();
		this.input_LanguageID = this.transform.Find("Input_LanguageID").GetComponent<InputField>();
		this.btn_Go = this.transform.Find("btn_Go").GetComponent<Button>();
		this.text_Welcome = this.transform.Find("text_Welcome").GetComponent<Text>();
		this.text_Show = this.transform.Find("text_Show").GetComponent<Text>();

		this.btn_Mask.onClick.AddListener(this.OnClickMask);
		this.btn_Language_CN.onClick.AddListener(this.OnClickLanguageCN);
		this.btn_Language_EN.onClick.AddListener(this.OnClickLanguageEN);
		this.btn_Language_JP.onClick.AddListener(this.OnClickLanguageJP);
		this.btn_Go.onClick.AddListener(this.OnClickGo);
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

	private void OnClickLanguageJP()
	{
		LanguageData.SetLanguage(LanguageType.Japanese);
	}

	private void OnClickGo()
	{
		if(this.input_LanguageID)
		{
			Debug.Log(this.input_LanguageID.text);
			LanguageData.SetTextByID(this.text_Show.gameObject, this.input_LanguageID.text);
		}
	}
}
