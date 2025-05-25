using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

public class CardUIManager : MonoBehaviour
{
    public static CardUIManager Instance;
    public GameObject personaje;
    public bool IsActive { get; private set; } = false;

    [Header("Animación Cartas")]
    public Vector2 startPosition = new Vector2(0, -500);
    public Vector2 finalPosition = Vector2.zero;
    public float animationDuration = 0.5f;

    [Header("UI de Cartas")]
    public CanvasGroup panelCards;
    public Button[] cardButtons;
    public Image[] cardBackgroundImages; 
    public Image[] cardIcons;      
    public TMP_Text[] cardTexts;

    [Header("Cartas Disponibles")]
    public List<CartaConfig> allCards;

    private List<CartaConfig> selectedCards = new List<CartaConfig>();

    private void Awake()
    {
        HideCards();
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        HideCards();
        if (GameManager.Instance != null)
            GameManager.Instance.OnLevelUp.AddListener(ShowCards);
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnLevelUp.RemoveListener(ShowCards);
        HideCards();
    }

    public void ShowCards()
    {
        try
        {
            IsActive = true;
            Time.timeScale = 0f;

            panelCards.alpha = 0f;
            panelCards.interactable = false;
            panelCards.blocksRaycasts = false;

            selectedCards = GetRandomCards(3);

            for (int i = 0; i < cardButtons.Length; i++)
            {
                if (i < selectedCards.Count)
                {
                    var card = selectedCards[i];

                    if (cardBackgroundImages.Length > i && card.backgroundSprite != null)
                        cardBackgroundImages[i].sprite = card.backgroundSprite;

                    if (cardIcons.Length > i)
                        cardIcons[i].sprite = card.icon;

                    cardButtons[i].gameObject.SetActive(true);
                    cardButtons[i].onClick.RemoveAllListeners();
                    cardButtons[i].onClick.AddListener(() =>
                    {
                        GameManager.Instance.ApplyCardUpgrade(card);
                        SyncCharacterAbilities();
                        HideCards();
                    });
                }
                else
                {
                    cardButtons[i].gameObject.SetActive(false);
                }
            }

            panelCards.blocksRaycasts = true;
            panelCards.interactable = true;

            StartCoroutine(AnimatePanel());
            AudioManager.Instance.PlaySFXFromEnum(AudioManager.SFX_Sounds.UPGRADE);
        }
        catch (Exception ex)
        {
            Debug.LogError("Error en ShowCards: " + ex);
        }
        finally
        {
            if (panelCards == null)
                HideCards();
        }
    }

    private IEnumerator AnimatePanel()
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / animationDuration;
            panelCards.alpha = Mathf.Lerp(0f, 1f, t);
            yield return null;
        }
        panelCards.alpha = 1f;
        panelCards.interactable = true;
        panelCards.blocksRaycasts = true;
    }

    public void SyncCharacterAbilities()
    {
        if (personaje == null) return;
        foreach (var h in personaje.GetComponentsInChildren<IHabilidad>())
            if (h is BaseAbility b) b.SyncUpgrades();
    }

    public void HideCards()
    {
        panelCards.interactable = false;
        panelCards.blocksRaycasts = false;
        panelCards.alpha = 0f;
        IsActive = false;
        Time.timeScale = 1f;
    }

    private List<CartaConfig> GetRandomCards(int count)
    {
        List<CartaConfig> validCards = new List<CartaConfig>();
        IHabilidad[] abilities = personaje.GetComponentsInChildren<IHabilidad>();

        foreach (var card in allCards)
        {
            if (card.cardType == CardType.Neutral)
            {
                validCards.Add(card);
                continue;
            }

            bool unlocked = card.targetAbility switch
            {
                AbilityType.Fire => personaje.GetComponentInChildren<DisparaFuego>()?.addHability ?? false,
                AbilityType.Planta => personaje.GetComponentInChildren<DisparaPlanta>()?.addHability ?? false,
                AbilityType.Rayo => personaje.GetComponentInChildren<DisparaRayo>()?.addHability ?? false,
                _ => false
            };
            if (!unlocked)
                continue;

            foreach (var ability in abilities)
            {
                if (ability.GetAbilityType() == card.targetAbility
                    && ability.CanApplyCard(card))
                {
                    validCards.Add(card);
                    break;
                }
            }
        }

        if (validCards.Count == 0)
            validCards.AddRange(allCards.FindAll(c => c.cardType == CardType.Neutral));

        var result = new List<CartaConfig>();
        var available = new List<CartaConfig>(validCards);
        while (result.Count < count && available.Count > 0)
        {
            int idx = Random.Range(0, available.Count);
            var pick = available[idx];
            if (!result.Contains(pick))
                result.Add(pick);
            available.RemoveAt(idx);
        }
        while (result.Count < count)
            result.Add(allCards.Find(c => c.cardType == CardType.Neutral));

        return result;
    }
}
