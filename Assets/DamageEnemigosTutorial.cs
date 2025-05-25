using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEnemigosTutorial : MonoBehaviour
{
    [Header("Targets")]
    public GameObject Disparo;
    public Transform shootPosition;

    [Header("Cadencia")]
    public float cadenciaMin = 1f;
    public float cadenciaMax = 3f;

    private Transform target;
    private float temporizador;
    private float proximoDisparo;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("player").transform;
        proximoDisparo = Random.Range(cadenciaMin, cadenciaMax);
    }

    void Update()
    {
        if (target == null)
            return;

        Vector2 direccion = target.position - transform.position;
        float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angulo);

        temporizador += Time.deltaTime;
        if (temporizador >= proximoDisparo)
        {
            Shoot();
            temporizador = 0f;
            proximoDisparo = Random.Range(cadenciaMin, cadenciaMax);
        }
    }

    void Shoot()
    {
        EventManager.TriggerEvent("EnemyShot");

        GameObject proyectil = Instantiate(Disparo, shootPosition.position, transform.rotation);
        Destroy(proyectil, 2f);
    }
}
