using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerLifeManager : MonoBehaviour
{
    public static PlayerLifeManager Instance; 

    [Header("Vidas")]
    public int maxLives = 3;
    private bool isInvincible = false;
    public float invincibleDuration = 0.5f; 
    private int currentLives;

    [Header("Eventos de UI")]
    public UnityEvent<int> OnLifeChanged = new UnityEvent<int>(); // Evento para UI corazones

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        currentLives = maxLives;
        OnLifeChanged.Invoke(currentLives);
    }

    public void LoseLife(int damage)
    {
        if (isInvincible) return; 

        currentLives = Mathf.Max(currentLives - damage, 0);
        OnLifeChanged.Invoke(currentLives);

        if (currentLives <= 0)
            EndGame();
        else
            StartCoroutine(InvincibilityCoroutine());
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleDuration);
        isInvincible = false;
    }

    public void GainLife(int heal)
    {
        currentLives = Mathf.Min(currentLives + heal, maxLives);
        OnLifeChanged?.Invoke(currentLives);
    }

    private void EndGame()
    {
        SceneManager.LoadScene("MenuInicio");
    }
}
