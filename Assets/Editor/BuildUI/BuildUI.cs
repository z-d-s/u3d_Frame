using DG.Tweening.Plugins.Core.PathCore;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BuildUI : EditorWindow
{
    [MenuItem("Tools/Build UI", false, 1001)]
    public static void AutoBuildUI()
    {
        EditorWindow.GetWindow<BuildUI>();
    }

    void OnGUI()
    {
        GUILayout.Label("选择一个UI 视图根节点");
        if (GUILayout.Button("生成代码"))
        {
            if (Selection.activeGameObject != null)
            {
                Debug.Log("开始生成...");
                CreatUISourceFile(Selection.activeGameObject);
                Debug.Log("生成结束");

                AssetDatabase.Refresh();
            }
        }

        if (Selection.activeGameObject != null)
        {
            GUILayout.Label(Selection.activeGameObject.name);
        }
        else
        {
            GUILayout.Label("没有选中的UI节点，无法生成");
        }
    }

    void OnSelectionChange()
    {
        this.Repaint();
    }

    //创建UISource文件的函数
    public static void CreatUISourceFile(GameObject selectGameObject)
    {
        string className = selectGameObject.name;
        StreamWriter sw;

        string dirPath = Application.dataPath + "/Scripts/Game/UIModule/" + className;
        if (Directory.Exists(dirPath) == false)
        {
            Directory.CreateDirectory(dirPath);
        }
        if (File.Exists(dirPath + "/" + className + ".cs"))
        {
            return;
        }

        sw = new StreamWriter(dirPath + "/" + className + ".cs");
        sw.WriteLine("using UnityEngine;\nusing System.Collections;\nusing UnityEngine.UI;\nusing System.Collections.Generic;\n");
        sw.WriteLine("public class " + className + " : BaseUI");

        sw.WriteLine("{");

        sw.WriteLine("\t" + "public override void Awake()");
        sw.WriteLine("\t" + "{");
        sw.WriteLine("\t\t" + "base.Awake();");
        sw.WriteLine("\t" + "}" + "\n");

        sw.WriteLine("\t" + "void Start()");
        sw.WriteLine("\t" + "{");
        sw.WriteLine("\t" + "}");

        sw.WriteLine("}");

        sw.Flush();
        sw.Close();

        Debug.Log("Generate : " + Application.dataPath + "/Scripts/Game/UIModule/" + className + ".cs");
    }
}
