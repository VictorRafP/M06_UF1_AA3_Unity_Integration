using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CambiodeFase : MonoBehaviour
{
    public BossControllerRayo BossController;
    public BossControllerFire BossControllerFire;
    public BossControllerPlanta BossControllerPlanta;
    public Animator animator;
    public int Type = 0;
    public BossFaseManager FaseManager;
    void Start()
    {
        animator.GetComponent<Animator>();
        FaseManager = FindObjectOfType<BossFaseManager>();

        BossController = FindObjectOfType<BossControllerRayo>();
        BossControllerFire = FindObjectOfType<BossControllerFire>();
        BossControllerPlanta = FindObjectOfType<BossControllerPlanta>();
        if (Type == 1)
        {
            animator.SetFloat("Fase", BossController.currentPhaseIndex - 1);
        }
        else if (Type == 2)
        {
            animator.SetFloat("Fase", BossControllerFire.currentPhaseIndex - 1);
        }
        else if (Type == 3)
        {
            animator.SetFloat("Fase", BossControllerPlanta.currentPhaseIndex - 1);
        }
    }

    void Update()
    {
        if (FaseManager._gemsRemaining <= 0 && FaseManager._currentPhase == 2)
        {
            animator.SetFloat("Fase", 3f);
        }
    }
}
