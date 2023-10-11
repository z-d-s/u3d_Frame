using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilTool
{
    /// <summary>
    /// 矫正旋转角度值 限定在0~360之间
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    public static float JiaoZheng(float angle)
    {
        while(angle > 360)
        {
            angle -= 360f;
        }

        while(angle < 0)
        {
            angle += 360f;
        }

        return angle;
    }

    public static string GetIDByIDAndLevel(string _id, int _level)
    {
        return _id + _level.ToString("d3");
    }

    /// <summary>
    /// 从路径中获取文件名
    /// </summary>
    /// <param name="path">路径</param>
    /// <param name="extension">扩展名</param>
    /// <returns></returns>
    public static string GetNameFromPath(string path, bool extension = false)
    {
        if(extension == false)
        {
            path = path.Substring(0, path.LastIndexOf("."));
        }
        string temp = path.Substring(0, path.LastIndexOf("/") + 1);
        path = path.Replace(temp, "");
        return path;
    }

    //                 0
    //       -45      ↑       45
    //                 |
    //                 |
    //      -90 ←-----o-----→ 90
    //                 |
    //                 |
    //       -135      ↓      135
    //             -180/180
    /// <summary>
    /// 获取角度 (目标点与基准点在世界坐标系下的夹角)
    /// </summary>
    /// <param name="oriPos">基准点</param>
    /// <param name="targetPos">目标点</param>
    /// <returns></returns>
    public static float GetAngle_XZ(Vector3 oriPos, Vector3 targetPos)
    {
        oriPos = new Vector3(oriPos.x, 0, oriPos.z);
        targetPos = new Vector3(targetPos.x, 0, targetPos.z);
        Vector3 offsetPos = targetPos - oriPos;
        float distance = Vector3.Distance(targetPos, oriPos);
        float _typeSin = offsetPos.x / distance;
        float _typeCos = offsetPos.z / distance;
        float angle_Sin = Mathf.Asin(offsetPos.x / distance) * (180 / Mathf.PI);
        float angle_Cos = Mathf.Asin(offsetPos.z / distance) * (180 / Mathf.PI);
        if (_typeSin >= 0 && _typeCos <= 0)
        {
            angle_Sin = 90 + (90 - angle_Sin);
        }
        else if (_typeSin <= 0 && _typeCos <= 0)
        {
            angle_Sin = -90 + (-90 - angle_Sin);
        }
        return angle_Sin;
    }

    /// <summary>
    /// 根据名字获取物体中的子物体(也会递归在子物体的子物体中查找)
    /// </summary>
    /// <param name="obj">根物体</param>
    /// <param name="name">要查找的子物体名字</param>
    /// <returns></returns>
    public static GameObject GetChildByName(GameObject obj, string name)
    {
        GameObject _obj = null;
        foreach (Transform go in obj.transform)
        {
            if (go.name == name)
            {
                _obj = go.gameObject;
                break;
            }
            else if (go.childCount > 0)
            {
                _obj = GetChildByName(go.gameObject, name);
                if (_obj != null)
                {
                    break;
                }
            }
        }
        return _obj;
    }
}
