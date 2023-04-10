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
