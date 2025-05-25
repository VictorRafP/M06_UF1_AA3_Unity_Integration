using UnityEngine;

public enum CardType
{
    Neutral,   // Ej: Heal
    Upgrade,   // Ej: FireUpgrade
    Evolution  // Ej: FireEvolution
}

[CreateAssetMenu(fileName = "NewCard", menuName = "Cards/New Card")]
public class CartaConfig : ScriptableObject
{
    [Header("Información de la Carta")]
    public string cardName;
    public Sprite icon;
    public Sprite backgroundSprite;

    [Header("Tipo de Carta")]
    public CardType cardType;
    public AbilityType targetAbility;

    [Header("Parámetros (según el tipo)")]
    // Cartas Neutral
    public float healAmount = 20f;

    // Cartas Upgrade:
    public float damageIncrease = 5f;
    public float sizeMultiplier = 1.1f;

    // Cartas Evolution:
    public int nivelRequerido = 3;
    public int additionalProjectiles = 4;
}
