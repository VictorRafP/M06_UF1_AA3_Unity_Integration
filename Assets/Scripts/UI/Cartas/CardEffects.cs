using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffects : MonoBehaviour
{
    public float currentDamage = 10f;
    public float currentSize = 1f;
    public int currentProjectiles = 1;
    public int currentLevel = 1;
    public bool evolved = false;

    public void Evolve()
    {
        if (!evolved)
        {
            evolved = true;
            Debug.Log("¡Habilidad evolucionada! Nivel: " + currentLevel);
        }
    }
}

