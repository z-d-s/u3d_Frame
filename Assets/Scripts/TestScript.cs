using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("Application.streamingAssetsPath" + Application.streamingAssetsPath);
        Debug.Log("Application.dataPath:" + Application.dataPath);
    }
}
