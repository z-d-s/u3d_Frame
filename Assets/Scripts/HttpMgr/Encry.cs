using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encry
{
    private string _encryKey;

    public Encry(string encryKey)
    {
        _encryKey = encryKey;
    }

    public void DoEncry(byte[] data)
    {
        int keyId = 0;
        for (int i = 0; i < data.Length; i++)
        {
            data[i] ^= (byte)_encryKey[keyId];
            keyId++;
            if (keyId >= _encryKey.Length)
            {
                keyId = 0;
            }
        }
    }
}
