using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BuildExcel : Editor
{
    [MenuItem("Tools/Build Excel", false, 2001)]
    public static void StartBuildExcel()
    {
        string absolutePath = Application.dataPath;
        int _start = absolutePath.LastIndexOf("/");
        int _length = absolutePath.Length;
        absolutePath = absolutePath.Remove(_start, _length - _start);

        RunBat(absolutePath);
    }

    /// <summary>
    /// 运行bat文件
    /// </summary>
    /// <param name="path">绝对路径</param>
    private static void RunBat(string path)
    {
        string batPath = path + "/Tables/ConvertExcel.bat";

        if (!File.Exists(batPath))
        {
            UnityEngine.Debug.LogError("文件不存在" + batPath);
            return;
        }

        try
        {
            //创建新进程实例
            Process process = new Process();
            FileInfo fileInfo = new FileInfo(batPath);
            process.StartInfo.WorkingDirectory = fileInfo.Directory.FullName;
            process.StartInfo.FileName = fileInfo.Name;
            process.StartInfo.CreateNoWindow = false;

            //启动进程
            process.Start();

            //等待进程执行完成
            process.WaitForExit();

            process.Close();

            UnityEngine.Debug.Log("=>转换表格完成");

            //拷贝表格类到工程
            CopyFilesToProject(path + "/Tables/CSharp", path + "/Assets/Scripts/Tables");
            //拷贝表格Json到工程
            CopyFilesToProject(path + "/Tables/Json", path + "/Assets/AssetsPackage/Tables");

            UnityEngine.Debug.Log("=>所有表格拷贝完成");
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError(e.Message);
        }

        AssetDatabase.Refresh();
        LanguageData.FillLanguageData(true);
        LanguageData.SetLanguage(LanguageData.type);
        BuildFileList.BuildEditorFileList();
    }

    /// <summary>
    /// 拷贝文件
    /// </summary>
    /// <param name="sourcePath">源文件夹路径</param>
    /// <param name="targetPath">目标文件夹路径</param>
    private static void CopyFilesToProject(string sourcePath, string targetPath)
    {
        DirectoryInfo dir = new DirectoryInfo(sourcePath);
        //获取目标路径下的所有文件
        FileInfo[] files = dir.GetFiles("*", SearchOption.AllDirectories);
        for (int i = 0; i < files.Length; ++i)
        {
            string fileName = files[i].Name;
            File.Copy(sourcePath + "/" + fileName, targetPath + "/" + fileName, true);
        }
    }
}