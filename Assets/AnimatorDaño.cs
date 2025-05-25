using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorDaño : MonoBehaviour
{
    public Animator animator;

    public void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.CompareTag("Fuego") || trigger.CompareTag("Planta") || trigger.CompareTag("Rayo"))
        {
            animator.SetTrigger("Hurt");
        }
    }
}
