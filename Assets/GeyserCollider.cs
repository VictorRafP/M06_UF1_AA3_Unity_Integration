using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeyserCollider : MonoBehaviour
{
    [Header("Daño al tocar columna")]
    [SerializeField] private int damageOnTouch = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("player"))
            PlayerLifeManager.Instance.LoseLife(damageOnTouch);
    }
}
