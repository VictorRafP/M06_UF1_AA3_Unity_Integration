using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Escudo : MonoBehaviour
{
    [Header("Referencia Escudo")]
    public GameObject escudo;

    [Header("Config Escudo")]
    public float duration = 3f;
    public float cooldown = 5f;

    [Header("UI Cooldown Escudo")]
    [SerializeField] private Slider cooldownSlider;
    [SerializeField] private TextMeshProUGUI cooldownText;

    private bool isOnCooldown = false;
    private float cooldownTimer;

    void Start()
    {
        if (escudo != null)
            escudo.SetActive(false);
        else
            Debug.LogError("No se asignó el objeto escudo en el inspector.");

        cooldownTimer = cooldown;
        if (cooldownSlider != null)
            cooldownSlider.maxValue = cooldown;
        UpdateUI();
    }

    void Update()
    {
        if (isOnCooldown)
        {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer > cooldown)
                cooldownTimer = cooldown;
            UpdateUI();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && !isOnCooldown)
        {
            StartCoroutine(ActivateShield());
        }
    }

    private IEnumerator ActivateShield()
    {
        escudo.SetActive(true);
        Debug.Log("Escudo activado");
        yield return new WaitForSeconds(duration);

        escudo.SetActive(false);
        Debug.Log("Escudo desactivado, iniciando cooldown");

        isOnCooldown = true;
        cooldownTimer = 0f;
        UpdateUI();

        yield return new WaitForSeconds(cooldown);

        isOnCooldown = false;
        cooldownTimer = cooldown;
        UpdateUI();
        Debug.Log("Cooldown terminado, escudo listo de nuevo");
    }

    private void UpdateUI()
    {
        if (cooldownSlider != null)
            cooldownSlider.value = cooldownTimer;

        if (cooldownText != null)
        {
            if (isOnCooldown)
            {
                float remaining = cooldown - cooldownTimer;
                cooldownText.text = Mathf.CeilToInt(remaining).ToString();
            }
            else
            {
                cooldownText.text = "CTRL";
            }
        }
    }
}
