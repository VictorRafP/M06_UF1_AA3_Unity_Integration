using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEnemigos : MonoBehaviour
{
    public int damage = 50;
    private int id;
    private plantDamage pD;

    private void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.CompareTag("circulo"))
        {
            HP_Circulo hpCirculo = trigger.GetComponent<HP_Circulo>();
            if (hpCirculo != null)
            {
                if (GetComponent<enemyMover>().ID == 1) {
                    pD = this.GetComponent<plantDamage>();
                    //if (pD == null) { Debug.Log("plant damage not existing");  }
                    if (pD != null) 
                    {
                        pD.enabled = true;
                    }


                    //plantDamage pD = trigger.GetComponent<plantDamage>();
                    //if (pD == null) { Debug.Log("Noob"); }
                    //pD.enabled = true;

                }
                else
                {
                    hpCirculo.RecibirDamageCirculo(damage);
                }
            }
        }
    }
}