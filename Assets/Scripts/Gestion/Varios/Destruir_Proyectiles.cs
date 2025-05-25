using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destruir_Proyectiles : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Proyectil"))
        {
            Destroy(other.gameObject);
        }
    }
}
