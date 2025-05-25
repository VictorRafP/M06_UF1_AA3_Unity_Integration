using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHabilidades : MonoBehaviour
{
    // Eliminamos la variable p�blica dmgHab
    public int currentDamage = 10;
    public bool fuego;
    public GameObject bomba;
    // M�todo para actualizar el da�o desde CartaConfig
    public void UpdateDamage(int newDamage)
    {
        currentDamage = newDamage;
    }

    private void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.CompareTag("Enemigo") || trigger.CompareTag("Boss"))
        {
            VidaEnemigo vidaEnemigo = trigger.GetComponent<VidaEnemigo>();
            if (vidaEnemigo != null)
            {
                vidaEnemigo.RecibirDamage(currentDamage); // Usamos currentDamage
            }
            HP_Boss hpBoss = trigger.GetComponent<HP_Boss>();
            if (hpBoss != null)
            {
                hpBoss.RecibirDamage(currentDamage);
            }
            if (fuego) {
                Debug.Log("SoyFuego");
                GameObject area = Instantiate(bomba, transform.position, Quaternion.identity);
                AudioManager.Instance.PlaySFXFromEnum(AudioManager.SFX_Sounds.FIRE_EXPLOSION);
                Destroy(area, 1.5f);
                Destroy(gameObject);
            }
        }
    }
}