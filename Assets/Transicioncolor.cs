using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transicioncolor : MonoBehaviour
{
    public Animator animator;
    public DisparaFuego fuego;
    public DisparaPlanta planta;
    public DisparaRayo rayo;
    
    void Update()
    {
        if (fuego.waitingForClick){
            animator.SetFloat("habilidad", 0);
            animator.SetFloat("habilidad", 1);
        }
        else if (planta.waitingForClick){
            animator.SetFloat("habilidad", 0);
            animator.SetFloat("habilidad", 2);
        }
        else if (rayo.waitingForClick){
            animator.SetFloat("habilidad", 0);
            animator.SetFloat("habilidad", 3);
        }
        else {
            animator.SetFloat("habilidad", 0);
        }
    }
}
