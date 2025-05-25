using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantaMov : MonoBehaviour
{
    public float speed;
    public float altura_min = -3.3f;
    public float altura_max = -2.5f;
    private Camera Cam;
    float direccionX;
    int posicionX = 5;
    public bool sprite;
    void Start()
    {
        Cam = Camera.main;
        direccionX = Cam.ScreenToWorldPoint(Input.mousePosition).x;
        if (direccionX < 0)
        {
            if (!sprite)
            {
                transform.position = new Vector2(-posicionX, altura_min);
            }
            else
            {
                transform.position = new Vector2(-posicionX, altura_max);
            }
        }
        else
        {
            if (!sprite)
            {
                transform.position = new Vector2(posicionX, altura_min);
            }
            else
            {
                transform.position = new Vector2(posicionX, altura_max);
            }
        }
    }

    void Update()
    {
        if (!sprite)
        {
            if (transform.position.y < altura_max)
            {
                Vector3 dir = new Vector3(0, 1, 0);
                transform.position += dir * speed * Time.deltaTime;
            }
        }
    }
}
