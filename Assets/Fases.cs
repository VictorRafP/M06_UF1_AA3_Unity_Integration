using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fases : MonoBehaviour
{
    public BossControllerRayo BossControllerRayo;
    public Animator animator;
    void Start()
    {
        BossControllerRayo = FindObjectOfType<BossControllerRayo>();
        animator = GetComponent<Animator>();

        if (BossControllerRayo.currentPhaseIndex == 1)
        {
            animator.SetTrigger("Fase2");
        }else if (BossControllerRayo.currentPhaseIndex == 2)
        {
            animator.SetTrigger("Fase3");
        }
    }

    void Update()
    {
        
    }
}
