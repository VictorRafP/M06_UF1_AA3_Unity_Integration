using UnityEngine;

public class VidaEnemigo : MonoBehaviour
{
    [Header("Vida y XP")]
    public int vidaMaxima = 50;
    public int xpOtorgada = 10;

    [Header("Evento al morir")]
    [Tooltip("Nombre del evento que se dispara al destruir este enemigo")]
    public string deathEventName = "TutorialEnemyDestroyed";

    private int vidaActual;
    private DamageFlash _damageFlash;

    void Start()
    {
        vidaActual = vidaMaxima;
        _damageFlash = GetComponent<DamageFlash>();
    }

    public void RecibirDamage(int dmg)
    {
        vidaActual = Mathf.Max(vidaActual - dmg, 0);
        if (vidaActual <= 0)
        {
            DestruirEnemigo();
        }
        else
        {
            _damageFlash.CallDamageFlash();
            AudioManager.Instance.PlaySFXFromEnum(AudioManager.SFX_Sounds.HIT);
        }
    }

    private void DestruirEnemigo()
    {
        EventManager.TriggerEvent(deathEventName);

        GameManager.Instance.GainXP(xpOtorgada);

        Destroy(gameObject);
    }
}