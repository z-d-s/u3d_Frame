using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(Text))]
public class LanguageText : MonoBehaviour
{
    public string key;
    private Text com_Text;

    public void Localize()
    {
        this.SetText();
    }

    private void Awake()
    {
        Debug.Log("=== LanguageText::Awake ===");
        this.com_Text = this.GetComponent<Text>();
    }

    private void Start()
    {
        Debug.Log("=== LanguageText::Start ===");
        this.SetText();
    }

    private void OnEnable()
    {
        this.SetText();
        Debug.Log("=== LanguageText::OnEnable ===");
        LanguageMgr.OnLocalize += Localize;
    }

    private void OnDisable()
    {
        Debug.Log("=== LanguageText::OnDisable ===");
        LanguageMgr.OnLocalize -= Localize;
    }

    private void SetText()
    {
        if (this.com_Text == null)
        {
            return;
        }

        if (string.IsNullOrEmpty(this.key))
        {
            return;
        }

        this.GetComponent<Text>().text = LanguageData.Get(this.key);
    }
}
