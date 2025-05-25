using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XpBarUI : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private Slider xpSlider;
    [SerializeField] private TextMeshProUGUI nivelText;

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnXPUpdate.AddListener(ActualizarUI);
            // Inicializa la UI con los valores actuales guardados en el GameManager
            ActualizarUI(GameManager.Instance.playerLevel, GameManager.Instance.playerXP, GameManager.Instance.xpRequired);
        }
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnXPUpdate.RemoveListener(ActualizarUI);
        }
    }

    public void ActualizarUI(int nivel, float xpActual, float xpRequerida)
    {
        if (xpSlider != null)
        {
            xpSlider.maxValue = xpRequerida;
            xpSlider.value = xpActual;
        }
        if (nivelText != null)
        {
            nivelText.text = nivel.ToString();
        }
    }
}
