using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(Text))]
public class LanguageText : MonoBehaviour
{
    public string ID;
    private Text com_Text;

    public void Localize()
    {
        this.SetText();
    }

    private void Awake()
    {
        this.com_Text = this.GetComponent<Text>();
    }

    private void Start()
    {
        this.SetText();
    }

    private void OnEnable()
    {
        this.SetText();
        LanguageData.OnLocalize += Localize;
    }

    private void OnDisable()
    {
        LanguageData.OnLocalize -= Localize;
    }

    private void SetText()
    {
        if (this.com_Text == null)
        {
            return;
        }

        if (string.IsNullOrEmpty(this.ID))
        {
            return;
        }

        this.GetComponent<Text>().text = LanguageData.GetText(this.ID);
    }

    public void SetTextID(string id)
    {
        this.ID = id;
        this.SetText();
    }
}
