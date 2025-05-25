using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plantDamage : MonoBehaviour
{
    public int damage = 15; 
    public float damageTimeCadence = 1f;
    public float currentTime = 0f;
    public HP_Circulo hpc;
    public GameObject circle;

    private void Start()
    {
        hpc = FindAnyObjectByType<HP_Circulo>();
    }
    void Update()
    {
    }

    private void FixedUpdate()
    {
        currentTime += Time.fixedDeltaTime;
        if (currentTime > damageTimeCadence)
        {
            hpc.RecibirDamageCirculo(damage);
            currentTime = 0f;
        }
    }
}
