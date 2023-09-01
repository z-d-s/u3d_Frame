using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BaseUI : MonoBehaviour
{
    public Dictionary<string, GameObject> view = new Dictionary<string, GameObject>();

    public virtual void Awake()
    {
        this.LoadObjects(this.gameObject);
    }

    private void LoadObjects(GameObject root, string path = "")
    {
        foreach (Transform tf in root.transform)
        {
            if (this.view.ContainsKey(path + tf.gameObject.name))
            {
                //LogHelper.LogWarning("Warning object is exist:" + path + tf.gameObject.name + "!");
                continue;
            }
            this.view.Add(path + tf.gameObject.name, tf.gameObject);

            this.LoadObjects(tf.gameObject, path + tf.gameObject.name + "/");
        }
    }

    public void AddButtonListener(string view_name, UnityAction onclick)
    {
        Button btn = this.view[view_name].GetComponent<Button>();
        if (btn == null)
        {
            LogHelper.LogWarning("UI_manager add_button_listener: not Button Component!");
            return;
        }

        btn.onClick.AddListener(onclick);
    }

    public void AddSliderListener(string view_name, UnityAction<float> on_value_changle)
    {
        Slider sl = this.view[view_name].GetComponent<Slider>();
        if (sl == null)
        {
            LogHelper.LogWarning("UI_manager add_slider_listener: not Slider Component!");
            return;
        }

        sl.onValueChanged.AddListener(on_value_changle);
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
