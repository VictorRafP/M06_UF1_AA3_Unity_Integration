using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PJPersistente : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
