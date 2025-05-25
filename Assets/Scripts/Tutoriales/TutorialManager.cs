using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    [Header("Tutorial Enemies and Spawn Points")]
    [Tooltip("Prefab del primer enemigo de tutorial (melee)")]
    public GameObject tutorialEnemyPrefab1;
    [Tooltip("Punto de spawn para el primer enemigo")]
    public Transform tutorialSpawnPoint1;

    [Tooltip("Prefab del segundo enemigo de tutorial (ranged)")]
    public GameObject tutorialEnemyPrefab2;
    [Tooltip("Punto de spawn para el segundo enemigo")]
    public Transform tutorialSpawnPoint2;

    [Tooltip("Prefab del tercer enemigo de tutorial (boss demo)")]
    public GameObject tutorialEnemyPrefab3;
    [Tooltip("Punto de spawn para el tercer enemigo")]
    public Transform tutorialSpawnPoint3;

    [Header("UI de Tutorial")]
    public GameObject tutorialPanel;
    public TextMeshProUGUI tutorialText;
    public Button nextButton;
    public Button skipButton;

    public TextMeshProUGUI pressSpaceText;
    public CanvasGroup pressSpaceCanvasGroup;

    public static bool IsInTutorial { get; private set; }
    private bool skipRequested = false;

    void Awake() 
    { 
        IsInTutorial = true; 
    }

    void Start()
    {
        pressSpaceCanvasGroup.alpha = 0;
        pressSpaceCanvasGroup.gameObject.SetActive(false);

        tutorialPanel.SetActive(false);
        skipButton.onClick.RemoveAllListeners();
        skipButton.onClick.AddListener(OnSkipTutorial);
        nextButton.onClick.RemoveAllListeners();

        StartCoroutine(RunTutorial());
    }

    private IEnumerator RunTutorial()
    {
        var shooter = FindObjectOfType<Disparos>();
        if (shooter != null) shooter.enabled = false;

        // Paso 0: Spawn primer enemigo y esperar trigger
        bool reached1 = false;
        void OnReached1() => reached1 = true;
        EventManager.StartListening("EnemyReachedCircle", OnReached1);
        SpawnEnemy(tutorialEnemyPrefab1, tutorialSpawnPoint1);
        yield return new WaitUntil(() => reached1 || skipRequested);
        EventManager.StopListening("EnemyReachedCircle", OnReached1);

        if (shooter != null) shooter.enabled = true;

        // Paso 1: Mostrar texto y esperar clic izquierdo
        yield return ShowAndWaitInput(
        "¡Un enemigo ha llegado!\nPulsa clic izquierdo para atacar.",

            KeyCode.Mouse0
        );

        // Paso 2: Esperar daño al círculo
        bool damaged = false;
        void OnDamaged() => damaged = true;
        EventManager.StartListening("CircleDamaged", OnDamaged);
        yield return new WaitUntil(() => damaged || skipRequested);
        EventManager.StopListening("CircleDamaged", OnDamaged);

        yield return new WaitForSeconds(0.5f);

        // Paso 2.1: Mostrar advertencia y texto “pulsa espacio”
        tutorialText.text = "¡El círculo ha sido dañado! Vigila que la barra de vida no descienda hasta 0.";
        tutorialPanel.SetActive(true);
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(1f);

        yield return ShowPressSpace("Aprieta Barra Espaciadora para seguir");

        tutorialPanel.SetActive(false);
        Time.timeScale = 1f;

        // --- Paso 3: Spawn segundo enemigo y esperar disparo ---
        bool shot = false;
        void OnShot() => shot = true;
        EventManager.StartListening("EnemyShot", OnShot);
        SpawnEnemy(tutorialEnemyPrefab2, tutorialSpawnPoint2);
        yield return new WaitUntil(() => shot || skipRequested);
        EventManager.StopListening("EnemyShot", OnShot);

        yield return new WaitForSeconds(0.1f);

        yield return ShowAndWaitInput(
            "¡Un enemigo dispara!\nPulsa Ctrl para activar el escudo.",
            KeyCode.LeftControl
        );

        // --- Paso 3 finalizado: esperar muerte de enemigo 2 ---
        bool dead2 = false;
        void OnDead2() => dead2 = true;
        EventManager.StartListening("TutorialEnemyDestroyed2", OnDead2);
        yield return new WaitUntil(() => dead2 || skipRequested);
        EventManager.StopListening("TutorialEnemyDestroyed2", OnDead2);

        yield return new WaitForSecondsRealtime(1f);

        // --- Paso 4: Preparar y spawnear heavy ---
        bool spawn3 = false;
        void OnReached3() => spawn3 = true;
        EventManager.StartListening("ThirdEnemyReached", OnReached3);
        SpawnEnemy(tutorialEnemyPrefab3, tutorialSpawnPoint3);
        yield return new WaitUntil(() => spawn3 || skipRequested);
        EventManager.StopListening("ThirdEnemyReached", OnReached3);

        tutorialText.text = "Estás a punto de subir de nivel.\nDerrota a este enemigo para ganar experiencia.";
        tutorialPanel.SetActive(true);
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(1f);
        yield return ShowPressSpace("Aprieta Barra Espaciadora para seguir");

        tutorialPanel.SetActive(false);
        Time.timeScale = 1f;

        // --- Paso 5: muerte heavy y tuto panel cartas ---
        bool dead3 = false;
        void OnDead3() => dead3 = true;
        EventManager.StartListening("TutorialEnemyDestroyed3", OnDead3);

        yield return new WaitUntil(() => dead3 || skipRequested);
        EventManager.StopListening("TutorialEnemyDestroyed3", OnDead3);

        tutorialText.text = "¡Has subido de nivel!\nAhora tienes las cartas de mejora, selecciona una.";
        tutorialPanel.SetActive(true);
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(1f);
        yield return ShowPressSpace("Pulsa Espacio para seleccionar una carta");

        tutorialPanel.SetActive(false);
        Time.timeScale = 1f;

        yield return new WaitUntil(() => CardUIManager.Instance != null && !CardUIManager.Instance.IsActive);

        yield return new WaitForSeconds(1f);

        // --- Paso 6: explicar ranuras de habilidades ---
        tutorialText.text = "En la esquina izquierda tienes 3 ranuras de habilidades.\n\nLas irás desbloqueando al liberar las gemas de cada jefe.";
        tutorialPanel.SetActive(true);
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(1f);
        yield return ShowPressSpace("Pulsa Espacio para seleccionar una carta");
        tutorialPanel.SetActive(false);
        Time.timeScale = 1f;

        yield return new WaitForSeconds(2f);

        // --- Paso 7: explicar la barra de oleada y transición final ---
        tutorialText.text = "La barra superior muestra cuánto debes aguantar\nhasta que llegue el jefe. ¡Sobrevive todo lo que puedas!";
        tutorialPanel.SetActive(true);
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(1f);
        yield return ShowPressSpace("Pulsa Espacio para seleccionar una carta");
        tutorialPanel.SetActive(false);
        Time.timeScale = 1f;

        GameManager.Instance.playerLevel = 0;
        GameManager.Instance.playerXP = 0f;
        GameManager.Instance.xpRequired = 50f; 
        GameManager.Instance.OnXPUpdate?.Invoke(
            GameManager.Instance.playerLevel,
            GameManager.Instance.playerXP,
            GameManager.Instance.xpRequired
        );

        EndTutorial();
    }

    [Header("UI Pulsing Settings")]
    [Tooltip("Número de ciclos de palpito completos por segundo")]
    public float pressSpacePulseSpeed = 0.5f;  // medio ciclo por segundo → un pulso completo cada 2 s

    private IEnumerator ShowPressSpace(string message)
    {
        // Prepara y muestra el texto
        pressSpaceText.text = message;
        pressSpaceCanvasGroup.alpha = 0f;
        pressSpaceCanvasGroup.gameObject.SetActive(true);

        float t = 0f;
        while (!skipRequested && !Input.GetKeyDown(KeyCode.Space))
        {
            // Pulso lento con fase inicial −π/2 para arrancar en 0
            float alpha = 0.5f * (1f + Mathf.Sin(2f * Mathf.PI * pressSpacePulseSpeed * t - Mathf.PI * 0.5f));
            pressSpaceCanvasGroup.alpha = alpha;
            t += Time.unscaledDeltaTime;
            yield return null;
        }

        // Limpia al salir
        pressSpaceCanvasGroup.alpha = 0f;
        pressSpaceCanvasGroup.gameObject.SetActive(false);
    }

    private IEnumerator ShowAndWaitNext(string message)
    {
        // Pone el texto y activa el panel
        tutorialText.text = message;
        tutorialPanel.SetActive(true);
        // Pausa el juego
        Time.timeScale = 0f;

        // Espera al click en el botón Next
        bool clicked = false;
        nextButton.onClick.AddListener(() => clicked = true);
        yield return new WaitUntil(() => clicked || skipRequested);
        nextButton.onClick.RemoveAllListeners();

        // Limpia y reanuda el juego
        tutorialPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    private IEnumerator ShowAndWaitInput(string message, KeyCode key)
    {
        tutorialText.text = message;
        tutorialPanel.SetActive(true);
        Time.timeScale = 0f;

        yield return new WaitUntil(() => Input.GetKeyDown(key) || skipRequested);

        tutorialPanel.SetActive(false);
        Time.timeScale = 1f;
    }


    private void SpawnEnemy(GameObject prefab, Transform spawnPoint)
    {
        if (prefab == null || spawnPoint == null)
        {
            Debug.LogError("TutorialManager: asigna prefab y spawnPoint en Inspector");
            return;
        }

        var enemy = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        enemy.tag = "TutorialEnemy";
    }

    private void EndTutorial()
    {
        foreach (var e in GameObject.FindGameObjectsWithTag("TutorialEnemy"))
            Destroy(e);

        tutorialPanel.SetActive(false);
        Time.timeScale = 1f;
        EventManager.TriggerEvent("TutorialCompleted");
        IsInTutorial = false;
    }

    public void OnSkipTutorial()
    {
        skipRequested = true;

        foreach (var e in GameObject.FindGameObjectsWithTag("TutorialEnemy"))
            Destroy(e);

        StopAllCoroutines();

        tutorialPanel.SetActive(false);
        Time.timeScale = 1f;

        EventManager.TriggerEvent("TutorialCompleted");

        IsInTutorial = false;
    }
}
