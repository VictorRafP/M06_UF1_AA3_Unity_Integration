using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayoMov : MonoBehaviour
{
    public float speed;
    int mover = -1;
    public Transform childAnimation; 

    void Start()
    {
        
    }

    private void Update()
    {
        Vector3 dir = new Vector3(0, mover, 0);
        transform.position += dir * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Suelo")
        {
            //animacion de choque
            mover = 0;
        }
        //if (collision.tag == "Enemigo")
        //{
        //    Destroy(gameObject);
        //}
    }

}
