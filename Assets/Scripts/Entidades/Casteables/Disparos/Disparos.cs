using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Disparos : MonoBehaviour
{
    public static bool abilityClicked = false;
    public short cargaMax = 5;
    public short cargaActual = 0;

    public GameObject Disparo;
    public Transform shootPosition;
    private Camera Cam;
    public float cadence;
    public float chargeTime;
    private float currentTime;

    void Start()
    {
        chargeTime = 1;
        cadence = 0.5f;
        Cam = Camera.main;
    }

    void Update()
    {
        if (cargaActual >= cargaMax)
        {
            cadence = 0.25f;
            cargaActual = 0;
        }
        currentTime += Time.deltaTime;

        if (MenuPausa.GameIsPaused ||
            (CardUIManager.Instance != null && CardUIManager.Instance.IsActive) ||
            EventSystem.current.IsPointerOverGameObject())
            return;

        if (abilityClicked)
        {
            abilityClicked = false;
            return;
        }

        if (Input.GetMouseButton(0) && currentTime >= cadence)
        {
            currentTime = 0;
            Shoot();
            AudioManager.Instance.PlaySFXFromEnum(AudioManager.SFX_Sounds.BASICSHOOT);
        }
    }

    public void Shoot()
    {
        Vector2 posicionDisparo = Cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direccion = posicionDisparo - (Vector2)transform.position;
        transform.up = direccion;

        GameObject proyectil = Instantiate(Disparo, shootPosition.position, transform.rotation);
        Destroy(proyectil, 2f);
    }

    public void ShootCharged()
    {
        Vector2 posicionDisparo = Cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direccion = posicionDisparo - (Vector2)transform.position;
        transform.up = direccion;

        GameObject proyectil = Instantiate(Disparo, shootPosition.position, transform.rotation);
        proyectil.transform.localScale *= 2;
        Destroy(proyectil, 2f);
    }
}
