using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class EnergyBall : MonoBehaviour
{
    public float velocidadMovimiento = 3f;
    public GameObject alertPrefab;
    private Vector3 destino;
    public float currentTime = 0f;
    public float waitTime = 2f;
    public GameObject playerGo;
    public Transform puntoA;
    public Transform puntoB;
    public GameObject rectanguloPrefab; // Sprite rectangular con escala inicial (1,1,1)
    public bool thunderCreated = false;
    public bool playerTracked = false;
    public float lifeTimeCounter = 0f;
    public float lifeTimeMax = 15f;
    public Vector3 playerPos;
    public float exitSpeed = 4.5f;

    public Animator animator;

    void Start()
    {

        playerGo = GameObject.FindGameObjectWithTag("player");
        AudioManager.Instance.PlaySFXFromEnum(AudioManager.SFX_Sounds.LIGHTNING_BOSS_BALL);
        destino = GenerarPosicionAleatoria();
    }

    void Update()
    {
        lifeTimeCounter += Time.deltaTime;

        if (playerGo.transform.position.x < transform.position.x)
        {
            animator.SetFloat("direccion", -4f);
        }
        else if (playerGo.transform.position.x >= transform.position.x)
        {
            animator.SetFloat("direccion", 4f);
        }

        if(lifeTimeCounter >= lifeTimeMax && !playerTracked && !thunderCreated)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(13,5,0), exitSpeed * Time.deltaTime);
            if(Vector3.Distance(transform.position, new Vector3(13, 5, 0)) < 0.01f)
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, destino, velocidadMovimiento * Time.deltaTime);
            if (Vector3.Distance(transform.position, destino) < 0.01f)
            {
                if (!playerTracked) 
                {
                    playerPos = playerGo.transform.position;
                    playerTracked = true;
                    AudioManager.Instance.PlaySFXFromEnum(AudioManager.SFX_Sounds.LIGHTNING_BOSS_BALL_CHARGE);
                    GameObject alert = Instantiate(alertPrefab, new Vector3(playerPos.x, -1.8f, 0), Quaternion.identity);
                    Destroy(alert, waitTime/2);
                }
                currentTime += Time.deltaTime;
                if(currentTime > waitTime/2 && !thunderCreated)
                {
                    AudioManager.Instance.PlaySFXFromEnum(AudioManager.SFX_Sounds.LIGHTNING_BOSS);
                    CrearRectangulo(transform.position, new Vector3(playerPos.x, -3, 0));
                    thunderCreated = true;
                }
                else if(currentTime > waitTime) 
                {
                    currentTime = 0;
                    destino = GenerarPosicionAleatoria();
                    playerTracked = false;
                    thunderCreated=false;
                }
            }
        }
        // Si llegó al destino, generar uno nuevo

    }

    private Vector3 GenerarPosicionAleatoria()
    {
        return new Vector3(
            Random.Range(-2.5f, 7.5f),
            Random.Range(1f, 4f),
            transform.position.z
        );
    }

    void CrearRectangulo(Vector3 desde, Vector3 hasta)
    {
        Vector3 direccion = (hasta - desde).normalized;
        Vector3 hastaExtendido = hasta + direccion * 5f;
        Vector3 centro = (desde + hastaExtendido) / 2f;
        float largo = Vector3.Distance(desde, hastaExtendido) /*direccion.magnitude*/;

        GameObject rectangulo = Instantiate(rectanguloPrefab, centro, Quaternion.identity);
        Destroy(rectangulo, 1f);

        // Escalamos en el eje Y (alto), manteniendo ancho original
        rectangulo.transform.up = direccion.normalized; // Esto rota el objeto para que su "arriba" apunte hacia el otro punto
        rectangulo.transform.localScale = new Vector3(.3f, largo, 1f); // Ajusta el alto (Y) para que cubra la distancia
    }
}
