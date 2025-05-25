using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DisparaFuego : BaseAbility
{
    public GameObject Fuego;
    private Camera Cam;
    public Transform shootPosition;
    public DisparaPlanta planta;
    public DisparaRayo rayo;

    [Header("Cooldown y Disparo")]
    public float fixedCooldown = 5f;
    public float CurrentTime = 0f;
    public float delayEntreProyectiles = 0.1f;
    public float velocidadProyectil = 10f;
    public bool waitingForClick = false;
    private bool isShooting = false;
    private char boton = 'Q';
    public bool addHability = false;

    [Header("Cooldown UI")]
    public Slider cooldownSlider;
    public TextMeshProUGUI cooldownText;

    protected override void Start()
    {
        base.Start();

        Cam = Camera.main;
        CurrentTime = fixedCooldown;
        if (effectHandler == null)
            effectHandler = GetComponent<CardEffects>();

        if (cooldownSlider != null)
            cooldownSlider.maxValue = fixedCooldown;
        if (cooldownText != null)
            cooldownText.text = boton.ToString();

        SyncUpgrades();
    }

    void Update()
    {
        if (addHability)
        {
            if (CurrentTime < fixedCooldown)
            {
                CurrentTime += Time.deltaTime;
                if (cooldownSlider != null)
                    cooldownSlider.value = CurrentTime;
                if (cooldownText != null)
                    cooldownText.text = Mathf.Ceil(fixedCooldown - CurrentTime).ToString();
            }
            else
            {
                if (cooldownText != null)
                    cooldownText.text = boton.ToString();
            }

            if (MenuPausa.GameIsPaused || (CardUIManager.Instance != null && CardUIManager.Instance.IsActive) || isShooting)
                return;

            if (Input.GetButtonDown("Fuego") && CurrentTime >= fixedCooldown && !planta.waitingForClick && !rayo.waitingForClick)
            {
                waitingForClick = true;
                AudioManager.Instance.PlaySFXFromEnum(AudioManager.SFX_Sounds.HABILITY);
                BaseAbility.activeAbility = this;
            }

            if (waitingForClick && Input.GetMouseButtonDown(1))
            {
                if (BaseAbility.activeAbility == this)
                {
                    Disparos.abilityClicked = true;
                    isShooting = true;
                    StartCoroutine(TirarFuegoConDelay());
                    AudioManager.Instance.PlaySFXFromEnum(AudioManager.SFX_Sounds.FIRE);
                }
                waitingForClick = false;
            }
        }
    }

    private IEnumerator TirarFuegoConDelay()
    {
        Vector2 posicionFuego = Cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direccionFuego = posicionFuego - (Vector2)transform.position;
        transform.up = direccionFuego;

        AbilityStats upgradeStats = null;
        if (GameManager.Instance.abilityUpgrades.ContainsKey(GetAbilityType()))
            upgradeStats = GameManager.Instance.abilityUpgrades[GetAbilityType()];

        int proyectiles = effectHandler != null ? effectHandler.currentProjectiles : 1;
        for (int i = 0; i < proyectiles; i++)
        {
            InstanciarProyectil(upgradeStats);
            yield return new WaitForSeconds(delayEntreProyectiles);
        }
        CurrentTime = 0;
        if (cooldownSlider != null)
            cooldownSlider.value = 0;
        isShooting = false;
    }

    private void InstanciarProyectil(AbilityStats upgradeStats)
    {
        GameObject proyectil = Instantiate(Fuego, shootPosition.position, transform.rotation);
        float finalSize = effectHandler != null ? effectHandler.currentSize : 1f;
        if (upgradeStats != null)
            finalSize *= upgradeStats.sizeMultiplier;
        proyectil.transform.localScale *= finalSize;

        Rigidbody2D rb = proyectil.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = transform.up * velocidadProyectil;
        }
        Destroy(proyectil, 2f);
    }

    public override AbilityType GetAbilityType()
    {
        return AbilityType.Fire;
    }
}
