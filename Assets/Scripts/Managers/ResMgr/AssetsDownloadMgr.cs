using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetsDownloadMgr : MonoBaseSingleton<AssetsDownloadMgr>
{
    public bool IsNeedDownload(string _assetName)
    {
        return false;
    }

    public byte[] DownloadSync(string _assetName)
    {
        return new byte[0];
    }

    public void DownloadAsync(string _assetName, System.Action _callFun)
    {

    }

    public void AddDownloadSetFlag(string _assetName)
    {

    }
}
