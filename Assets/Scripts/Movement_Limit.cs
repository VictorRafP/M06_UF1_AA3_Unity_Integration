using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_Limit : MonoBehaviour
{
    public float xMax = 8.55f;
    public float xMin = -8.55f;
    public float yMax = 4f;
    // Update is called once per frame
    void Update()
    {
        if (transform.position.x > xMax)
        {
            Debug.Log("Me he pasado");
            transform.position = new Vector2(xMax, transform.position.y);
        }
        if (transform.position.x < xMin)
        {
            Debug.Log("Me he pasado");
            transform.position = new Vector2(xMin, transform.position.y);
        }
        if (transform.position.y > yMax)
        {
            Debug.Log("Me he pasado");
            transform.position = new Vector2(transform.position.x, yMax);
        }
    }
}
