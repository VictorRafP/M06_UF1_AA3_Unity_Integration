using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public int dmg = 10;

    private void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.CompareTag("Enemigo") || trigger.CompareTag("TutorialEnemy"))
        {
            VidaEnemigo vidaEnemigo = trigger.GetComponent<VidaEnemigo>();
            if (vidaEnemigo != null)
            {
                vidaEnemigo.RecibirDamage(dmg);
                Destroy(gameObject);
            }
        }
    }
}
