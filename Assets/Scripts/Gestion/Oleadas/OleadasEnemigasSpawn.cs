//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class OleadasEnemigasSpawn : MonoBehaviour
//{
//    [Header("Tipos de enemigos")]
//    public GameObject Flyer;
//    public GameObject Runner;
//    public GameObject Heavy;

//    [Header("Parámetros de spawn")]
//    public int enemys_min = 3;
//    public int enemys_max = 9;
//    public float spawn_max_x = 10f;
//    public float spawn_max_y = 6f;

//    private int enemys_actuales = 0;
//    private bool canSpawn = false;
//    private bool isTutorial = false;

//    void Awake()
//    {
//        // Detecta si hay un TutorialManager en la escena
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

//    void Start()
//    {
//        // Si no hay tutorial, arrancamos directo
//        canSpawn = !isTutorial;
//    }

//    void Update()
//    {
//        if (!canSpawn) return;

//        if (enemys_actuales <= 0)
//            CrearEnemys();
//    }

//    public void CrearEnemys()
//    {
//        int total = Random.Range(enemys_min, enemys_max + 1);
//        enemys_actuales = total;

//        for (int i = 0; i < total; i++)
//        {
//            Vector3 posicion = new Vector3(
//                Random.Range(-spawn_max_x, spawn_max_x),
//                Random.Range(-spawn_max_y, spawn_max_y),
//                0f
//            );

//            if (Vector3.Distance(posicion, Vector3.zero) > 3f)
//            {
//                SpawnAndAssign(Flyer, posicion);
//                SpawnAndAssign(Runner, posicion);
//                SpawnAndAssign(Heavy, posicion);
//            }
//        }
//    }

//    private void SpawnAndAssign(GameObject prefab, Vector3 position)
//    {
//        if (prefab == null) return;
//        GameObject temp = Instantiate(prefab, position, Quaternion.identity);
//        var manager = temp.GetComponent<Enemy_Manager>();
//        if (manager != null)
//            manager.gestor = this;
//    }
//}
