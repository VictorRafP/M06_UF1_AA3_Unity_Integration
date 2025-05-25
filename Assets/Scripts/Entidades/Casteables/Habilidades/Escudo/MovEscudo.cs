using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MovEscudo : MonoBehaviour
{
    public static bool estaActivo;

    [Header("Duraciones")]
    public float duration = 3f;
    public float cooldown = 5f;

    [Header("UI Cooldown Escudo")]
    [SerializeField] private Slider cooldownSlider;
    [SerializeField] private TextMeshProUGUI cooldownText;

    private bool isOnCooldown = false;
    private float cooldownTimer;

    void Start()
    {
        estaActivo = false;
        gameObject.SetActive(false);

        cooldownTimer = cooldown;
        if (cooldownSlider != null)
        {
            cooldownSlider.maxValue = cooldown;
            cooldownSlider.value = cooldown;
        }
        UpdateUI();
    }

    void Update()
    {
        if (isOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                cooldownTimer = 0f;
                isOnCooldown = false;
            }
            UpdateUI();
        }

        if (!isOnCooldown && Input.GetKeyDown(KeyCode.LeftControl))
        {
            ActivarEscudo();
        }
    }

    private void UpdateUI()
    {
        if (cooldownSlider != null)
        {
            cooldownSlider.value = cooldownTimer;
        }

        if (cooldownText != null)
        {
            if (isOnCooldown)
            {
                cooldownText.text = Mathf.CeilToInt(cooldownTimer).ToString();
            }
            else
            {
                cooldownText.text = "CTRL";
            }
        }
    }

    public void ActivarEscudo()
    {
        estaActivo = true;
        gameObject.SetActive(true);

        isOnCooldown = true;
        cooldownTimer = cooldown;

        UpdateUI();

        CancelInvoke(nameof(DesactivarEscudo));
        Invoke(nameof(DesactivarEscudo), duration);
    }

    private void DesactivarEscudo()
    {
        estaActivo = false;
        gameObject.SetActive(false);
    }
}
