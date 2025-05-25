using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBoss : MonoBehaviour
{
    public int dmg = 10;

    private void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.CompareTag("Boss"))
        {
            HP_Boss hpBoss = trigger.GetComponent<HP_Boss>();
            if (hpBoss != null)
            {
                hpBoss.RecibirDamage(dmg);
                Destroy(gameObject);
            }
        }
    }
}