public interface IHabilidad
{
    AbilityType GetAbilityType();
    void ApplyCardConfig(CartaConfig card);
    bool CanApplyCard(CartaConfig card);
}
