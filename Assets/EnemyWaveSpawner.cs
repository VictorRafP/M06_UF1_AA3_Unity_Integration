using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveSpawner : MonoBehaviour
{
    [Header("Prefabs de Enemigos (0=Flyer,1=Runner,2=Heavy)")]
    public GameObject[] enemyPrefabs;

    [Header("Posiciones de Spawn")]
    [Tooltip("Distancia X fuera de pantalla")]
    public float limitX = 10f;
    [Tooltip("Altura mínima para Flyers")]
    public float flyerMinY = 4f;
    [Tooltip("Altura máxima para Flyers")]
    public float flyerMaxY = 8f;

    [Header("Parámetros de Oleada")]
    [Tooltip("Mínimo de enemigos por oleada")]
    public int minEnemiesPerWave = 1;
    [Tooltip("Máximo de enemigos por oleada")]
    public int maxEnemiesPerWave = 5;
    [Tooltip("Cooldown mínimo (s) entre oleadas")]
    public float minSpawnCooldown = 2f;
    [Tooltip("Cooldown máximo (s) entre oleadas")]
    public float maxSpawnCooldown = 5f;

    [Header("Delay de la Primera Oleada")]
    [Tooltip("Tiempo a esperar antes de la primera oleada")]
    public float firstSpawnDelay = 5f;

    [Header("Separación mínima entre spawns")]
    [Tooltip("Distancia mínima entre dos enemigos spawnados en la misma oleada")]
    public float minSpawnSpacing = 1.5f;

    // Tutorial logic
    private bool isTutorial;
    private bool canSpawn;

    // Internos
    private float spawnTimer;
    private bool firstWaveDone = false;

    void Awake()
    {
        isTutorial = (FindObjectOfType<TutorialManager>() != null);
    }

    void OnEnable()
    {
        if (isTutorial)
            EventManager.StartListening("TutorialCompleted", OnTutorialCompleted);
    }

    void OnDisable()
    {
        if (isTutorial)
            EventManager.StopListening("TutorialCompleted", OnTutorialCompleted);
    }

    void Start()
    {
        // Si no hay tutorial, arrancamos ya; si hay, esperamos al evento
        canSpawn = !isTutorial;
        // Para la primera oleada usamos un delay distinto
        spawnTimer = firstSpawnDelay;
    }

    void Update()
    {
        if (!canSpawn) return;

        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            SpawnWave();
            firstWaveDone = true;
            ResetSpawnTimer();
        }
    }

    private void OnTutorialCompleted()
    {
        canSpawn = true;
        // cuando acabe el tutorial, la primera oleada también sale tras firstSpawnDelay
        spawnTimer = firstSpawnDelay;
        firstWaveDone = false;
    }

    private void ResetSpawnTimer()
    {
        // Sólo tras la primera oleada usamos cooldown aleatorio normal
        spawnTimer = Random.Range(minSpawnCooldown, maxSpawnCooldown);
    }

    private void SpawnWave()
    {
        int count = Random.Range(minEnemiesPerWave, maxEnemiesPerWave + 1);
        List<Vector3> used = new List<Vector3>();

        for (int i = 0; i < count; i++)
        {
            int choice = Random.Range(0, enemyPrefabs.Length);
            Vector3 pos = FindNonOverlappingPosition(choice, used);
            used.Add(pos);

            Instantiate(enemyPrefabs[choice], pos, Quaternion.identity);
        }
    }

    private Vector3 FindNonOverlappingPosition(int index, List<Vector3> used)
    {
        Vector3 pos;
        int attempts = 0;
        do
        {
            float x = (Random.value < 0.5f) ? -limitX : limitX;
            float y;
            switch (index)
            {
                case 0: y = Random.Range(flyerMinY, flyerMaxY); break;
                case 1: y = -2.5f; break;
                case 2: y = -2f; break;
                default: y = 0f; break;
            }
            pos = new Vector3(x, y, 0f);
            attempts++;
        }
        while (attempts < 10 && used.Exists(o => Vector3.Distance(o, pos) < minSpawnSpacing));

        return pos;
    }
}
