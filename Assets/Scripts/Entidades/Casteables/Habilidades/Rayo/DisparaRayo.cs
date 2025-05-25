using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisparaRayo : BaseAbility
{
    public GameObject Rayo;
    private Camera Cam;
    public float alturaRayo = 10f;
    public DisparaFuego fuego;
    public DisparaPlanta planta;

    // Timer del cooldown.
    public float CurrentTime = 0f;
    public Slider cooldownSlider;
    public TextMeshProUGUI cooldownText;

    [Header("Cooldown Fijo")]
    public float fixedCooldown = 5f;
    public bool waitingForClick = false;
    private char boton = 'E';
    public bool addHability = false;

    protected override void Start()
    {
        base.Start();

        Cam = Camera.main;
        CurrentTime = fixedCooldown;
        cooldownSlider.maxValue = fixedCooldown;
        if (cooldownText != null)
            cooldownText.text = boton.ToString();

        if (effectHandler == null)
        {
            effectHandler = GetComponent<CardEffects>();
            if (effectHandler == null)
            {
                Debug.LogError("No se encontró CardEffects en " + gameObject.name);
            }
        }

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
                CurrentTime += Time.deltaTime;
                cooldownSlider.value = CurrentTime;
                if (cooldownText != null)
                {
                    cooldownText.text = Mathf.Ceil(fixedCooldown - CurrentTime).ToString();
                    cooldownText.gameObject.SetActive(true);
                }
            }
            else
            {
                if (cooldownText != null)
                    cooldownText.text = boton.ToString();
            }

            if (Input.GetButtonDown("Rayo") && CurrentTime >= fixedCooldown && !fuego.waitingForClick && !planta.waitingForClick)
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
                    TirarRayo();
                    AudioManager.Instance.PlaySFXFromEnum(AudioManager.SFX_Sounds.LIGHTNING);
                }
                waitingForClick = false;
            }
        }
    }

    public void TirarRayo()
    {
        // Consulta mejoras para la habilidad Rayo
        AbilityStats upgradeStats = null;
        if (GameManager.Instance.abilityUpgrades.ContainsKey(GetAbilityType()))
            upgradeStats = GameManager.Instance.abilityUpgrades[GetAbilityType()];

        float posicionX = Cam.ScreenToWorldPoint(Input.mousePosition).x;
        Vector2 posicionRayo = new Vector2(posicionX, alturaRayo);
        GameObject instanciaRayo = Instantiate(Rayo, posicionRayo, transform.rotation);
        instanciaRayo.transform.localScale *= new Vector2(0.65f, 1f);

        if (upgradeStats != null)
        {
            instanciaRayo.transform.localScale *= new Vector2(upgradeStats.sizeMultiplier, 1f);
        }

        Destroy(instanciaRayo, 1);

        CurrentTime = 0;
        cooldownSlider.value = 0;
        Debug.Log("Rayo lanzado.");
    }

    // Identifica esta habilidad como tipo Rayo.
    public override AbilityType GetAbilityType()
    {
        return AbilityType.Rayo;
    }
}
