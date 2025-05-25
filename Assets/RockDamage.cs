using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockDamage : MonoBehaviour
{
    [Header("Daño al impactar")]
    [SerializeField] private int damageOnImpact = 1;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("player"))
        {
            PlayerLifeManager.Instance.LoseLife(damageOnImpact);
        }
    }
}
