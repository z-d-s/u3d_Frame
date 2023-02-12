using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TestAssetBundle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadRes()
    {
    }

    IEnumerator UnityWebRequestAssetBundle_GetAssetBundle(string url, Action<AssetBundle> loadABFinishedAction)
    {
        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(string.Format("UnityWebRequest url:{0} error !!!", url));
        }
        else
        {
            AssetBundle ab = DownloadHandlerAssetBundle.GetContent(request);
            if (loadABFinishedAction != null)
            {
                loadABFinishedAction(ab);
            }
        }
    }

    private void LoadABFinishedAction(AssetBundle ab)
    {
        GameObject.Find("Cube").GetComponent<Renderer>().material.mainTexture = (Texture)ab.LoadAsset("PolygonPrototype_Texture_01");
    }
}
