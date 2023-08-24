using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageText : MonoBehaviour
{
    public string key;
    public void Localize()
    {
        this.GetComponent<Text>().text = "";
    }

    private void Start()
    {
        this.GetComponent<Text>().text = "";
    }

    private void OnEnable()
    {
        //LanguageMgr.OnLocalize += Localize;
    }

    private void OnDisable()
    {
        //LanguageMgr.OnLocalize -= Localize;
    }
}
