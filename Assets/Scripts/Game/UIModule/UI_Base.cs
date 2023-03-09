using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_Base : MonoBehaviour
{
    public Dictionary<string, GameObject> view = new Dictionary<string, GameObject>();

    private void load_all_object(GameObject root, string path)
    {
        foreach (Transform tf in root.transform)
        {
            if (this.view.ContainsKey(path + tf.gameObject.name))
            {
                //Utils.LogWarning("Warning object is exist:" + path + tf.gameObject.name + "!");
                continue;
            }
            this.view.Add(path + tf.gameObject.name, tf.gameObject);
            load_all_object(tf.gameObject, path + tf.gameObject.name + "/");
        }

    }

    public virtual void Awake()
    {
        this.load_all_object(this.gameObject, "");
    }


    public void add_button_listener(string view_name, UnityAction onclick)
    {
        Button bt = this.view[view_name].GetComponent<Button>();
        if (bt == null)
        {
            Utils.LogWarning("UI_manager add_button_listener: not Button Component!");
            return;
        }

        bt.onClick.AddListener(onclick);
    }

    public void add_slider_listener(string view_name, UnityAction<float> on_value_changle)
    {
        Slider s = this.view[view_name].GetComponent<Slider>();
        if (s == null)
        {
            Utils.LogWarning("UI_manager add_slider_listener: not Slider Component!");
            return;
        }

        s.onValueChanged.AddListener(on_value_changle);
    }
}
