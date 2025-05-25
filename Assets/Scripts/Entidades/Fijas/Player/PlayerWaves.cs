using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWaves : MonoBehaviour
{
    public MovEscudo escudo;
    private float cooldownTimer = 0f;

    void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && cooldownTimer <= 0f)
        {
            escudo.ActivarEscudo();
            AudioManager.Instance.PlaySFXFromEnum(AudioManager.SFX_Sounds.SHIELD);
            cooldownTimer = 5f;
            Debug.Log("Escudo preparado");
        }
    }
}