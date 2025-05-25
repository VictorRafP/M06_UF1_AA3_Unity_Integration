using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisparaPlanta : BaseAbility
{
    [Header("Referencia del Proyectil")]
    public GameObject Planta;
    public Transform shootPosition;
    public DisparaFuego fuego;
    public DisparaRayo rayo;

    [Header("Cooldown y UI")]
    public float fixedCooldown = 5f;
    public float CurrentTime = 5f;
    public Slider cooldownSlider;
    public TextMeshProUGUI cooldownText;

    private bool habilidadBloqueada = false;
    public bool waitingForClick = false;
    private char boton = 'W';
    public bool addHability = false;

    protected override void Start()
    {
        base.Start();

        if (shootPosition == null)
        {
            shootPosition = transform;
        }
        CurrentTime = fixedCooldown;
        if (cooldownSlider != null)
            cooldownSlider.maxValue = fixedCooldown;
        if (cooldownText != null)
            cooldownText.text = boton.ToString();

        if (effectHandler == null)
            effectHandler = GetComponent<CardEffects>();

        SyncUpgrades();
    }

    void Update()
    {
        if (MenuPausa.GameIsPaused || (CardUIManager.Instance != null && CardUIManager.Instance.IsActive))
            return;
        if (addHability)
        {
            if (CurrentTime < fixedCooldown)
            {
                habilidadBloqueada = true;
                CurrentTime += Time.deltaTime;
                if (cooldownSlider != null)
                    cooldownSlider.value = CurrentTime;
                if (cooldownText != null)
                {
                    cooldownText.text = Mathf.Ceil(fixedCooldown - CurrentTime).ToString();
                    cooldownText.gameObject.SetActive(true);
                }
            }
            else
            {
                habilidadBloqueada = false;
                if (cooldownText != null)
                    cooldownText.text = boton.ToString();
            }

            // Al pulsar el botón "Planta" y si la habilidad está lista, se activa la espera para disparar
            if (Input.GetButtonDown("Planta") && !habilidadBloqueada && CurrentTime >= fixedCooldown && !fuego.waitingForClick && !rayo.waitingForClick)
            {
                waitingForClick = true;
                AudioManager.Instance.PlaySFXFromEnum(AudioManager.SFX_Sounds.HABILITY);
                Debug.Log("Planta preparada. Esperando disparo.");
                BaseAbility.activeAbility = this;
            }

            // Si se está en espera y se hace clic, se dispara
            if (waitingForClick && Input.GetMouseButtonDown(1))
            {
                if (BaseAbility.activeAbility == this)
                {
                    Disparos.abilityClicked = true;
                    TirarPlanta();
                    AudioManager.Instance.PlaySFXFromEnum(AudioManager.SFX_Sounds.PLANT);
                }
                waitingForClick = false;
            }
        }
    }

    public void TirarPlanta()
    {
        // Consulta las mejoras para la habilidad de Planta en el GameManager
        AbilityStats upgradeStats = null;
        if (GameManager.Instance.abilityUpgrades.ContainsKey(GetAbilityType()))
            upgradeStats = GameManager.Instance.abilityUpgrades[GetAbilityType()];

        GameObject instancia = Instantiate(Planta, shootPosition.position, transform.rotation);

        // Se calcula el tamaño final del proyectil combinando el valor de stats base del effectHandler + la mejora acumulada
        float finalSize = (effectHandler != null ? effectHandler.currentSize : 1f);
        if (upgradeStats != null)
            finalSize *= upgradeStats.sizeMultiplier;
        instancia.transform.localScale *= finalSize;

        Destroy(instancia, 3f);

        // Reinicia el cooldown
        CurrentTime = 0;
        if (cooldownSlider != null)
            cooldownSlider.value = 0;
        habilidadBloqueada = true;
        Debug.Log("Planta disparada.");
    }

    public override AbilityType GetAbilityType()
    {
        return AbilityType.Planta;
    }
}
