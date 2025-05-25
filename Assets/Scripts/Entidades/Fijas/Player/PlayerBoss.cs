using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerBoss : MonoBehaviour
{
    [Header("Escudo")]
    public MovEscudo escudo;
    private float cooldownTimer = 0f;

    [Header("Invencibilidad")]
    public float invincibleDuration = 1f;
    [HideInInspector] public bool isInvincible = false;

    [Header("UI Cooldown Escudo")]
    [SerializeField] private Slider cooldownSlider;
    [SerializeField] private TextMeshProUGUI cooldownText;

    void Start()
    {
        cooldownTimer = 0f;

        if (cooldownSlider != null)
            cooldownSlider.maxValue = escudo.cooldown;

        UpdateCooldownUI();
    }

    void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer < 0f)
                cooldownTimer = 0f;
            UpdateCooldownUI();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && cooldownTimer <= 0f)
        {
            escudo.ActivarEscudo();
            AudioManager.Instance.PlaySFXFromEnum(AudioManager.SFX_Sounds.SHIELD);

            cooldownTimer = escudo.cooldown;
            UpdateCooldownUI();
        }
    }

    private void UpdateCooldownUI()
    {
        if (cooldownSlider != null)
            cooldownSlider.value = cooldownTimer;

        if (cooldownText != null)
        {
            if (cooldownTimer > 0f)
                cooldownText.text = Mathf.CeilToInt(cooldownTimer).ToString();
            else
                cooldownText.text = "CTRL"; 
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isInvincible && other.CompareTag("Proyectil"))
            return;

        if (other.CompareTag("Proyectil"))
        {
            StartCoroutine(InvincibleCoroutine());
            PlayerLifeManager.Instance.LoseLife(1);
            Destroy(other.gameObject);
            AudioManager.Instance.PlaySFXFromEnum(AudioManager.SFX_Sounds.HIT_CHARACTER);

            var dmgFlash = GetComponent<DamageFlash>();
            if (dmgFlash != null) dmgFlash.CallDamageFlash();

            var feedback = FindObjectOfType<HitFeedbackController>();
            if (feedback != null)
                feedback.PlayHitFeedback();
        }

        if (other.CompareTag("VidaUp"))
        {
            PlayerLifeManager.Instance.GainLife(1);
            Destroy(other.gameObject);
        }
    }

    private IEnumerator InvincibleCoroutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleDuration);
        isInvincible = false;
    }
}
