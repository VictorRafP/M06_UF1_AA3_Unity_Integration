using System;
using UnityEngine;

public abstract class BaseAbility : MonoBehaviour, IHabilidad
{
    public static BaseAbility activeAbility = null;

    [Header("Handler de efectos de carta")]
    public CardEffects effectHandler;

    [Header("Valores base de la habilidad")]
    protected float baseDamage;
    protected float baseSize;
    protected int baseProjectiles;

    protected virtual void Awake()
    {
        if (effectHandler == null)
            effectHandler = GetComponent<CardEffects>();

        baseDamage = effectHandler.currentDamage;
        baseSize = effectHandler.currentSize;
        baseProjectiles = effectHandler.currentProjectiles;

        if (GameManager.Instance != null)
            GameManager.Instance.OnLevelUp.AddListener(SyncUpgrades);
    }

    protected virtual void Start()
    {
        SyncUpgrades();
    }

    protected virtual void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnLevelUp.RemoveListener(SyncUpgrades);
    }

    public abstract AbilityType GetAbilityType();

    public virtual bool CanApplyCard(CartaConfig card)
    {
        if (GameManager.Instance.abilityUpgrades.TryGetValue(GetAbilityType(), out var stats))
        {
            if (card.cardType == CardType.Evolution)
                return stats.level >= card.nivelRequerido && !effectHandler.evolved;
            if (card.cardType == CardType.Upgrade)
                return stats.level < card.nivelRequerido;
            return true;
        }
        else
        {
            if (card.cardType == CardType.Evolution)
                return 1 >= card.nivelRequerido && !effectHandler.evolved;
            if (card.cardType == CardType.Upgrade)
                return 1 < card.nivelRequerido;
            return true;
        }
    }
    
    public virtual void ApplyCardConfig(CartaConfig card)
    {
        GameManager.Instance.ApplyCardUpgrade(card);
    }

    public virtual void SyncUpgrades()
    {
        if (GameManager.Instance.abilityUpgrades
            .TryGetValue(GetAbilityType(), out var stats))
        {
            effectHandler.currentDamage = baseDamage + stats.damage;
            effectHandler.currentSize = baseSize * stats.sizeMultiplier;
            effectHandler.currentProjectiles = baseProjectiles + stats.additionalProjectiles;
            effectHandler.currentLevel = stats.level;

            if (stats.evolved && !effectHandler.evolved)
                effectHandler.Evolve();
        }
        else
        {
            effectHandler.currentDamage = baseDamage;
            effectHandler.currentSize = baseSize;
            effectHandler.currentProjectiles = baseProjectiles;
            effectHandler.currentLevel = 1;
            effectHandler.evolved = false;
        }
    }
}
