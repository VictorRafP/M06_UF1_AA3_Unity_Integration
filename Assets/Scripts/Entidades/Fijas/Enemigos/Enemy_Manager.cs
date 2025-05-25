//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Enemy_Manager : MonoBehaviour
//{
//    public GameObject Flyer;
//    public GameObject Runner;
//    public GameObject Heavy;
//    public float limit_X = 10;
//    public float limit_Y = 6;
//    int random;
//    public OleadasEnemigasSpawn gestor;

//    public int actual_flayer_enemies = 0;

//    public bool spawned; // Comprueba si ha hecho spawn
//    public float currentTimeSpawn = 0;
//    public float minTimeSpawn = 2;
//    public float spawnCooldown = 0; // Tiempo hasta volver a hacer spawn
//    private float difficulty;
//    public float minDifficulty;
//    public float maxDifficulty;
//    private float spawnTime;
//    public float minSpawnTime;
//    public float maxSpawnTime;

//    private bool canSpawn = false;
//    private bool isTutorial = false;

//    void Awake()
//    {
//        // Detecta si hay un TutorialManager activo en escena
//        isTutorial = (FindObjectOfType<TutorialManager>() != null);
//    }

//    void OnEnable()
//    {
//        if (isTutorial)
//            EventManager.StartListening("TutorialCompleted", OnTutorialCompleted);
//    }

//    void OnDisable()
//    {
//        if (isTutorial)
//            EventManager.StopListening("TutorialCompleted", OnTutorialCompleted);
//    }

//    private void OnTutorialCompleted()
//    {
//        canSpawn = true;
//    }

//    // Start is called before the first frame update
//    void Start()
//    {
//        // Si NO estamos en tutorial, arrancamos directamente
//        canSpawn = !isTutorial;
//    }

//    public float RandomXGenerator()
//    {
//        random = Random.Range(0, 2);
//        return (random == 1) ? 12.8f : -12.8f;
//    }

//    public void FlyerSpawner()
//    {
//        Vector3 position = new Vector3(RandomXGenerator(), Random.Range(0, limit_Y / 2), 0);
//        Instantiate(Flyer, position, Quaternion.identity);
//    }

//    public void RunnerSpawner()
//    {
//        Vector3 position = new Vector3(RandomXGenerator(), -2.5f, 0);
//        Instantiate(Runner, position, Quaternion.identity);
//    }

//    public void HeavySpawner()
//    {
//        Vector3 position = new Vector3(RandomXGenerator(), -2f, 0);
//        Instantiate(Heavy, position, Quaternion.identity);
//    }

//    void Update()
//    {
//        if (!canSpawn) return;
//        Spawner();
//    }

//    private void Spawner()
//    {
//        CooldownSpawn();
//        if (spawned)
//        {
//            currentTimeSpawn += Time.deltaTime;
//            if (currentTimeSpawn >= minTimeSpawn)
//                endSpawn();
//        }
//        if (spawnCooldown <= 0)
//            Spawn();
//    }

//    private void CooldownSpawn()
//    {
//        if (spawnCooldown > 0)
//        {
//            spawnCooldown -= Time.deltaTime;
//            if (spawnCooldown < 0)
//                spawnCooldown = 0;
//        }
//    }

//    private void Spawn()
//    {
//        currentTimeSpawn = 0;
//        DifficultySpawn();
//        FlyerSpawner();
//        RunnerSpawner();
//        HeavySpawner();
//        spawned = true;
//    }

//    private void endSpawn()
//    {
//        currentTimeSpawn = 0;
//        spawned = false;
//    }

//    private void DifficultySpawn()
//    {
//        minDifficulty = 1;
//        maxDifficulty = 2;
//        difficulty = minDifficulty + Random.value * (maxDifficulty - minDifficulty);

//        minSpawnTime = 3;
//        maxSpawnTime = 5;
//        spawnTime = minSpawnTime + Random.value * (maxSpawnTime - minSpawnTime);

//        spawnCooldown = difficulty * spawnTime;
//    }
//}
