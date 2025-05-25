using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parallaxScroll : MonoBehaviour
{

    int i;
    public float speed = 1;
    float direccion;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        i--;
     transform.position = new Vector3(2250+i*speed, transform.position.y, transform.position.z);
    }
}
