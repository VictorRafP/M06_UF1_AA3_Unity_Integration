using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoDisparo : MonoBehaviour
{
    public float speed;
    public Rigidbody2D RbDisparo;

    void Start()
    {
        RbDisparo = GetComponent<Rigidbody2D>();
        RbDisparo.AddForce(transform.up * speed);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Suelo")
        {
            //animacion de choque
            Destroy(gameObject);
        }
        //if (collision.tag == "Enemigo")
        //{
        //    Destroy(gameObject);
        //}
    }
}
