using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPersistent : MonoBehaviour
{
    private static UIPersistent _instance;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;

        transform.SetParent(null);

        DontDestroyOnLoad(gameObject);
    }
}
