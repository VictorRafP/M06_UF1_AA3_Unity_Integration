using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPausa : MonoBehaviour
{
    public static MenuPausa Instance;
    public static bool GameIsPaused = false;

    [Header("Panel de Pausa")]
    [SerializeField] private GameObject menuPausaUI;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        menuPausaUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuPausaUI.activeSelf) Resume();
            else Pause();
        }
    }

    public void Pause()
    {
        menuPausaUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void Resume()
    {
        // Siempre ocultamos el menú de pausa
        menuPausaUI.SetActive(false);
        GameIsPaused = false;

        // Si hay un TutorialManager activo y su panel está abierto,
        // NO restauramos el timeScale para no interrumpir el tutorial
        var tm = FindObjectOfType<TutorialManager>();
        if (tm != null && tm.tutorialPanel.activeSelf)
        {
            // Salimos sin tocar Time.timeScale
            return;
        }

        // En cualquier otro caso (no hay tutorial o ya pasó),
        // sí reiniciamos el tiempo
        Time.timeScale = 1f;
    }



    public void RegresarMenuInicioClick() => SceneManager.LoadScene("MenuInicio");

    public void ExitGame() => Application.Quit();
}
