using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Practice001_Translate : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        if(Keyboard.current.wKey.isPressed)
        {
            //Debug.Log("=== W key is pressed! ===");
            this.transform.Translate(new Vector3(0f, 0f, 1f));
        }
    }
}
