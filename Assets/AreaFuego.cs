using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaFuego : MonoBehaviour
{
    float growspeed = 5f;
    void Start()
    {
        transform.localScale *= 0.1f;
    }
    void Update()
    {
        Vector3 growing = new Vector3(growspeed, growspeed, growspeed);
        transform.localScale += growing * Time.deltaTime;
    }
}
