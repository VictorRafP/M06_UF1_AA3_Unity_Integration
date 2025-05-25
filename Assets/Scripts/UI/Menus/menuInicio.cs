using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class menuInicio : MonoBehaviour
{
    private string autosavePath => Path.Combine(Application.persistentDataPath, "autosave.json");

    [Header("Buttons")]
    public Button continueButton;
    public TextMeshProUGUI continueLabel;
    public Button newGameButton;

    public Animator fadeanimator;

    [Header("Confirm Popup")]
    public GameObject confirmPopup;
    public Button yesButton;
    public Button noButton;

    void Start()
    {
        // Botones principales
        continueButton.onClick.AddListener(OnContinueClicked);
        newGameButton.onClick.AddListener(OnNewGameClicked);

        // Popup de confirmación
        yesButton.onClick.AddListener(ConfirmNewGame);
        noButton.onClick.AddListener(() => confirmPopup.SetActive(false));

        RefreshContinueButton();
        confirmPopup.SetActive(false);
    }

    private void RefreshContinueButton()
    {
        bool hasAuto = File.Exists(autosavePath);
        continueButton.interactable = hasAuto;
        continueLabel.color = hasAuto ? Color.white : Color.gray;
    }

    private void OnContinueClicked()
    {
        // Carga autosave.json (-1 indica autosave)
        GameManager.Instance.LoadFromSlot(-1);
    }

    private void OnNewGameClicked()
    {
        if (File.Exists(autosavePath))
        {
            // Si hay autosave, pide confirmación
            confirmPopup.SetActive(true);
        }
        else
        {
            // Si no hay nada, arranca sin preguntar
            StartNewGame();
        }
    }

    private void ConfirmNewGame()
    {
        // Borra el autosave y arranca de cero
        File.Delete(autosavePath);
        StartNewGame();
    }

    private void StartNewGame()
    {
        // Opción de slot “1” o lo que uses para autosave
        fadeanimator.SetTrigger("Fade");
        StartCoroutine(FadeIn(1f));
        GameManager.Instance.NewGame(1);
    }

    public IEnumerator FadeIn(float duration)
    {
        yield return new WaitForSeconds(duration);
    }

    public void CerrarPopUp()
    {
        confirmPopup.SetActive(false);
    }

    public void OpcionesClick()
    {
        SceneManager.LoadScene("MenuOpciones");
    }

    public void InfoClick()
    {
        SceneManager.LoadScene("MenuControles");
    }

    public void DebugClick()
    {
        SceneManager.LoadScene("DebugMenu");
    }

    public void CreditosClick()
    {
        SceneManager.LoadScene("Creditos");
    }

    public void RegresarMenuClick()
    {
        SceneManager.LoadScene("MenuInicio");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
