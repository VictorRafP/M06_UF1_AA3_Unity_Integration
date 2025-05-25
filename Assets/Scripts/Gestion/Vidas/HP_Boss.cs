using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP_Boss : MonoBehaviour
{
    [SerializeField] private float vida;
    [SerializeField] private float maxVida = 100;
    [SerializeField] private HP_Bar hpBar;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color hitColor = Color.white;

    private Color originalColor;
    private DamageFlash _damageFlash;  

    public float GetCurrentHealth() => vida;
    public float GetMaxHealth() => maxVida;

    private void Start()
    {
        vida = maxVida;
        originalColor = spriteRenderer.color;
        hpBar.InicializarBarraVida(maxVida);
        _damageFlash = GetComponent<DamageFlash>();  
    }

    public void RecibirDamage(int dmgBoss)
    {
        vida = Mathf.Max(vida - dmgBoss, 0);
        hpBar.CambiarVidaActual(vida);

        if (_damageFlash != null)
        {
            _damageFlash.CallDamageFlash();
        }
    }
}
