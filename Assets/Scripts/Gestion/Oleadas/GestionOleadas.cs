using UnityEngine;
using UnityEngine.UI;

public class GestionOleadas : MonoBehaviour
{
    [SerializeField] private Slider timerSlider;
    [SerializeField] private float maxTime = 20f;

    private float currentTime = 0f;
    private bool isTutorial;
    private bool canCount = false;

    void Awake()
    {
        isTutorial = FindObjectOfType<TutorialManager>() != null;
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
        currentTime = 0f;
        ConfigureSlider();
        // arrancamos solo si NO hay tutorial
        canCount = !isTutorial;
    }

    void Update()
    {
        if (!canCount) return;

        currentTime += Time.deltaTime;
        UpdateSlider();

        if (currentTime >= maxTime)
        {
            currentTime = 0f;
            UpdateSlider();
            GameManager.Instance.LoadNextScene();
        }
    }

    private void OnTutorialCompleted()
    {
        canCount = true;
    }

    private void ConfigureSlider()
    {
        if (timerSlider == null) return;
        timerSlider.minValue = 0f;
        timerSlider.maxValue = maxTime;
        timerSlider.value = 0f;
    }

    private void UpdateSlider()
    {
        if (timerSlider == null) return;
        timerSlider.value = currentTime;
    }
}
