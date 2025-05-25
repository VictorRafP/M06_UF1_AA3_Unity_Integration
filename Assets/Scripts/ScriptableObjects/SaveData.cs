using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public enum SaveType { Auto, Manual }
    public SaveType saveType;

    // Datos globales
    public int playerLevel;
    public float playerXP;
    public float xpRequired;
    public List<string> appliedCardNames;
    public List<AbilityStats> abilityStatsList;

    // Snapshot de escena
    public string sceneName;
    public float playTimeSeconds;

    public int currentSceneIndex;
    // public List<EnemyState>  enemies;       // snapshot completo (per a futur)
    // public PlayerState       playerState;   // idem
}
