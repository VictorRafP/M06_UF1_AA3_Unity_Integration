using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.Audio;
using System.IO;
using System.Linq;
using System;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Datos del Jugador")]
    public int playerLevel = 1;
    public float playerXP = 0f;
    public float xpRequired = 50f;

    [Header("Mejoras Aplicadas")]
    public Dictionary<AbilityType, AbilityStats> abilityUpgrades = new Dictionary<AbilityType, AbilityStats>();
    public List<CartaConfig> appliedCards = new List<CartaConfig>();


    [Header("UI Events")]
    public UnityEvent<int, float, float> OnXPUpdate; // (nivel, xpActual, xpRequerida)
    public UnityEvent OnLevelUp;
    
    [Header("Audio")]
    [Tooltip("Mixer con el Low-Pass expuesto")]
    public AudioMixer audioMixer;

    [Header("Secuencia Escenas")]
    public SceneSequence sequenceAsset;

    [Header("Todas las Cartas disponibles")]
    public List<CartaConfig> allCards;  // Poner todas las cartas que tenemos en el juego (sirve como biblioteca para cuando queramos continuar el game)

    [Header("Guardado")]
    public int activeSlot = 1;

    [Header("Tiempo de Juego")]
    public float playTimeSeconds = 0f;

    private int currentSceneIndex = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        OnXPUpdate ??= new UnityEvent<int, float, float>();
        OnLevelUp ??= new UnityEvent();
    }

    void Start()
    {
        OnXPUpdate?.Invoke(playerLevel, playerXP, xpRequired);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Update()
    {
        if (CardUIManager.Instance == null || !CardUIManager.Instance.IsActive)
        {
            playTimeSeconds += Time.unscaledDeltaTime;
        }
    }


    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1f;

        // Reiniciar pausa y UI de cartas
        MenuPausa.GameIsPaused = false;
        var cards = FindObjectOfType<CardUIManager>();
        if (cards != null && cards.IsActive)
            cards.HideCards();

        var cardUI = FindObjectOfType<CardUIManager>();
        if (cardUI != null)
            cardUI.SyncCharacterAbilities();

        // Sólo autosave si esta escena está en sequenceAsset.sceneNames
        if (sequenceAsset != null &&
            sequenceAsset.sceneNames != null &&
            Array.IndexOf(sequenceAsset.sceneNames, scene.name) >= 0)
        {
            AutoSave();
        }
    }

    public void LoadNextScene()
    {
        if (sequenceAsset == null || sequenceAsset.sceneNames == null)
        {
            Debug.LogError("SequenceAsset no asignado");
            return;
        }

        if (currentSceneIndex >= sequenceAsset.sceneNames.Length)
        {
            Debug.Log("¡Todas las escenas ya han sido jugadas!");
            return;
        }

        string nextScene = sequenceAsset.sceneNames[currentSceneIndex];

        if (SceneManager.GetActiveScene().name == nextScene)
        {
            currentSceneIndex++;
            if (currentSceneIndex >= sequenceAsset.sceneNames.Length)
            {
                Debug.Log("¡Todas las escenas ya han sido jugadas!");
                return;
            }
            nextScene = sequenceAsset.sceneNames[currentSceneIndex];
        }

        AutoSave();

        SceneManager.LoadScene(nextScene);
        currentSceneIndex++;
    }

    private void AutoSave()
    {
        var data = CreateSaveDataFromGameManager();
        string json = JsonUtility.ToJson(data, true);
        string path = Path.Combine(Application.persistentDataPath, "autosave.json");
        File.WriteAllText(path, json);
        Debug.Log($"[AutoSave] {path}");
    }

    public void NewGame(int slot)
    {
        activeSlot = slot;                      // 1. Selecciona el slot de guardado
        ResetGameData();                        // 2. Vuelve al estado inicial
        currentSceneIndex = 0;                  // 3. Reiniciar la escena en la que esta
        SaveToSlot(activeSlot);                 // 4. Sobrescribe el guardado con la partida limpia
        LoadNextScene();                        // 5. Carga la primera oleada
    }

    public void GainXP(float amount)
    {
        playerXP += amount;
        while (playerXP >= xpRequired)
        {
            playerXP -= xpRequired;
            playerLevel++;
            xpRequired = CalculateXPRequired(playerLevel);
            OnLevelUp?.Invoke();
        }
        OnXPUpdate?.Invoke(playerLevel, playerXP, xpRequired);
    }

    private float CalculateXPRequired(int level)
    {
        return 110f * Mathf.Pow(1.4f, level - 1);
    }

    public void ApplyCardUpgrade(CartaConfig card)
    {
        appliedCards.Add(card);
        if (card.targetAbility == AbilityType.Heal)
        {
            if (HP_Circulo.Instance != null)
            {
                HP_Circulo.Instance.Curar(card.healAmount);
                Debug.Log("Carta Heal aplicada: " + card.cardName + " cura " + card.healAmount + " puntos.");
            }
            else
            {
                Debug.LogWarning("No se encontró HP_Circulo.Instance.");
            }
            return;
        }
        if (!abilityUpgrades.ContainsKey(card.targetAbility))
        {
            abilityUpgrades[card.targetAbility] = new AbilityStats();
        }
        AbilityStats stats = abilityUpgrades[card.targetAbility];
        switch (card.cardType)
        {
            case CardType.Neutral:
                stats.healAmount += card.healAmount;
                Debug.Log("Aplicada carta Neutral: " + card.cardName + " | Heal +" + card.healAmount);
                break;
            case CardType.Upgrade:
                stats.damage += card.damageIncrease;
                stats.sizeMultiplier *= card.sizeMultiplier;
                stats.level++;
                Debug.Log("Aplicada carta Upgrade: " + card.cardName + " | Damage +" + card.damageIncrease + ", Size x" + card.sizeMultiplier + ", nivel " + stats.level);
                break;
            case CardType.Evolution:
                if (stats.level >= card.nivelRequerido)
                {
                    stats.additionalProjectiles += card.additionalProjectiles;
                    stats.evolved = true;
                    Debug.Log("Aplicada carta Evolution: " + card.cardName + " | Additional Projectiles +" + card.additionalProjectiles);
                }
                else
                {
                    Debug.Log("No se cumple el nivel requerido para la evolución de " + card.cardName);
                }
                break;
        }
    }

    public void ResetGameData()
    {
        playerLevel = 1;
        playerXP = 0f;
        xpRequired = 50f;
        appliedCards.Clear();
        abilityUpgrades.Clear();
        OnXPUpdate?.Invoke(playerLevel, playerXP, xpRequired);
    }

    private SaveData CreateSaveDataFromGameManager()
    {
        return new SaveData
        {
            playerLevel = playerLevel,
            playerXP = playerXP,
            xpRequired = xpRequired,
            currentSceneIndex = currentSceneIndex,
            sceneName = SceneManager.GetActiveScene().name,
            playTimeSeconds = playTimeSeconds,
            appliedCardNames = appliedCards.Select(c => c.cardName).ToList(),
            abilityStatsList = abilityUpgrades.Values.ToList()
        };
    }

    public bool SaveToSlot(int slot)
    {
        var data = CreateSaveDataFromGameManager();
        string json = JsonUtility.ToJson(data, true);  
        string path = Path.Combine(Application.persistentDataPath, $"save{slot}.json");
        File.WriteAllText(path, json);
        Debug.Log($"[Save] Saved slot {slot} @ {path}");
        return true;
    }


    public bool LoadFromSlot(int slot)
    {
        string path = Path.Combine(Application.persistentDataPath,
            slot == -1 ? "autosave.json" : $"save{slot}.json");

        if (!File.Exists(path))
            return false;

        var data = JsonUtility.FromJson<SaveData>(File.ReadAllText(path));

        playerLevel = data.playerLevel;
        playerXP = data.playerXP;
        xpRequired = data.xpRequired;
        playTimeSeconds = data.playTimeSeconds;
        appliedCards = data.appliedCardNames
            .Select(name => allCards.First(c => c.cardName == name)).ToList();
        abilityUpgrades = data.abilityStatsList
            .ToDictionary(s => s.abilityType, s => s);

        OnXPUpdate?.Invoke(playerLevel, playerXP, xpRequired);
        SyncAllCharacterAbilities();

        SceneManager.LoadScene(data.sceneName);

        // if (data.saveType == SaveData.SaveType.Auto)
        //     StartCoroutine(RestoreFullScene(data));

        currentSceneIndex = data.currentSceneIndex;

        return true;
    }


    private void ApplySaveDataToGameManager(SaveData data)
    {
        playerLevel = data.playerLevel;
        playerXP = data.playerXP;
        xpRequired = data.xpRequired;
        playTimeSeconds = data.playTimeSeconds;

        appliedCards = data.appliedCardNames
            .Select(name => allCards.First(c => c.cardName == name))
            .ToList();

        abilityUpgrades = data.abilityStatsList
            .ToDictionary(s => s.abilityType, s => s);

        OnXPUpdate?.Invoke(playerLevel, playerXP, xpRequired);

        SyncAllCharacterAbilities();

        if (!string.IsNullOrEmpty(data.sceneName) && sequenceAsset != null)
        {
            int idx = Array.IndexOf(sequenceAsset.sceneNames, data.sceneName);
            if (idx >= 0)
                currentSceneIndex = idx + 1;
        }
    }

    private void SyncAllCharacterAbilities()
    {
        var player = GameObject.FindWithTag("player");
        if (player == null) return;

        foreach (var h in player.GetComponentsInChildren<IHabilidad>())
        {
            if (h is BaseAbility b)
                b.SyncUpgrades();
        }
    }

    public bool HasSlot(int slot)
    {
        string path = Path.Combine(Application.persistentDataPath, $"save{slot}.json");
        return File.Exists(path);
    }

    public bool HasAnySlot() => HasSlot(1) || HasSlot(2) || HasSlot(3);

    public void DeleteSlot(int slot)
    {
        string path = Path.Combine(Application.persistentDataPath, $"save{slot}.json");
        if (File.Exists(path))
            File.Delete(path);

        if (activeSlot == slot)
        {
            ResetGameData();
            currentSceneIndex = 0;
        }
    }
}

[System.Serializable]
public class AbilityStats
{
    public AbilityType abilityType;

    public float damage = 0f;
    public float healAmount = 0f;
    public float sizeMultiplier = 1f;
    public int additionalProjectiles = 0;
    public bool evolved = false;
    public int level = 1;
}