﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum FireBossState { Idle, Attack1, Attack2, Attack3, Death }

public class BossControllerFire : MonoBehaviour
{
    public float winTimeCelebration = 10000f;
    public float endWinCelebration = 0;
    public bool ultimoBoss = false;

    public Animator fadeanimator;

    public VidaEnemigo tipo;

    public FireBossState currentState = FireBossState.Idle;

    [Header("Tiempo antes de Fase")]
    [SerializeField] private float initialDelay = 2f;
    [SerializeField] private float transitionDelay = 1f;

    [System.Serializable]
    public class BossPhase
    {
        public string phaseName;
        public float[] attackProbabilities; // Attack1, Attack2, Attack3
    }

    [SerializeField] private BossPhase[] phases;
    [SerializeField] private HP_Boss hpBoss;

    public Animator animator;
    public int currentPhaseIndex = 0;

    [Header("Habilidades")]
    [SerializeField] private FireballAttack Attack1;
    [SerializeField] private GeyserAttack Attack2;
    [SerializeField] private MeteorRainAttack Attack3;

    // Duración de cada ataque
    [Header("Duración de ataques")]
    public float durationAttack1 = 2f;
    public float durationAttack2 = 2.5f;
    public float durationAttack3 = 3f;

    public BossFaseManager phaseManager;
    private bool inCollectPhase = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        hpBoss = GetComponent<HP_Boss>();
        tipo = GetComponent<VidaEnemigo>();

        StartCoroutine(BeginAfterDelay());
    }

    private IEnumerator BeginAfterDelay()
    {
        // animator.SetTrigger("Intro"); (Cuando tengamos animacion)

        yield return new WaitForSeconds(initialDelay);
        ChangeState(FireBossState.Idle);
    }
    void Update()
    {
        if (hpBoss != null)
        {
            CheckBoss(); // Verifica vida y fases
        }

    }

    // MÁQUINA DE ESTADOS 
    public void ChangeState(FireBossState newState)
    {
        if (inCollectPhase || currentState == FireBossState.Death) return;

        currentState = newState;
        Debug.Log("Estado actual: " + currentState);

        switch (currentState)
        {
            case FireBossState.Idle:
                //animator.SetTrigger("Idle");
                ChooseAttack();
                break;
            case FireBossState.Attack1:
                //animator.SetTrigger("Ataque1");
                Attack1.LaunchAttack();
                StartCoroutine(ExecuteAttack(FireBossState.Attack1, durationAttack1));
                break;
            case FireBossState.Attack2:
                //animator.SetTrigger("Attack2");
                Attack2.LaunchAttack();
                StartCoroutine(ExecuteAttack(FireBossState.Attack2, durationAttack2));
                break;
            case FireBossState.Attack3:
                //animator.SetTrigger("Ataque3");
                Attack3.LaunchAttack();
                StartCoroutine(ExecuteAttack(FireBossState.Attack3, durationAttack3));
                break;
            case FireBossState.Death:
                //animator.SetTrigger("Death");
                Destroy(gameObject);
                break;
        }
    }

    private void ChooseAttack()
    {
        BossPhase currentPhase = phases[currentPhaseIndex];
        List<FireBossState> validAttacks = new List<FireBossState>();

        for (int i = 0; i < currentPhase.attackProbabilities.Length; i++)
        {
            if (currentPhase.attackProbabilities[i] > 0)
            {
                validAttacks.Add(FireBossState.Attack1 + i);
            }
        }

        if (validAttacks.Count == 0) return;

        int attackIndex = Choose(currentPhase.attackProbabilities);
        Debug.Log(validAttacks.Count);
        FireBossState chosenAttack = validAttacks[attackIndex];
        ChangeState(chosenAttack);
    }

    private int Choose(float[] probs)
    {
        float total = 0f;
        foreach (float prob in probs) total += prob;

        float randomPoint = Random.Range(0f, total);
        for (int i = 0; i < probs.Length; i++)
        {
            if (randomPoint < probs[i])
                return i;
            else
                randomPoint -= probs[i];
        }
        return probs.Length - 1;
    }

    private IEnumerator ExecuteAttack(FireBossState attackState, float duration)
    {
        yield return new WaitForSeconds(duration);
        yield return new WaitForSeconds(transitionDelay);
        ChangeState(FireBossState.Idle);
    }

    private void CheckBoss()
    {
        if (hpBoss.GetCurrentHealth() <= hpBoss.GetMaxHealth() * 0.66f && currentPhaseIndex == 0)
        {
            AudioManager.Instance.PlaySFXFromEnum(AudioManager.SFX_Sounds.BASICSHOOT);
            currentPhaseIndex = 1;
            inCollectPhase = true;
            animator.SetTrigger("Death");
            StartCoroutine(BossOut(0.5f,0));
            Debug.Log("¡Fase 2 activada!");
        }
        else if (hpBoss.GetCurrentHealth() <= hpBoss.GetMaxHealth() * 0.33f && currentPhaseIndex == 1)
        {
            AudioManager.Instance.PlaySFXFromEnum(AudioManager.SFX_Sounds.BASICSHOOT);
            currentPhaseIndex = 2;
            inCollectPhase = true;
            animator.SetTrigger("Death");
            StartCoroutine(BossOut(0.5f, 1));
            Debug.Log("¡Fase 3 activada!");
        }

        if (hpBoss.GetCurrentHealth() <= 0)
        {
            AudioManager.Instance.PlaySFXFromEnum(AudioManager.SFX_Sounds.HIT);
            currentPhaseIndex = 3;
            inCollectPhase = true;
            animator.SetTrigger("Death");
            StartCoroutine(BossOut(0.5f, 2));
            ChangeState(FireBossState.Death);
        }

        if (phaseManager.BossAcabado)
        {
            fadeanimator.SetTrigger("Fade");
            StartCoroutine(FadeIn(1f));
        }
    }

    public IEnumerator BossOut(float duration, int phase)
    {
        yield return new WaitForSeconds(duration);
        phaseManager.StartCollectPhase(phase);
    }

    public IEnumerator FadeIn(float duration)
    {
        yield return new WaitForSeconds(duration);
        GameManager.Instance.LoadNextScene();
    }

    public void OnCollectPhaseEnded()
    {
        inCollectPhase = false;
        ChangeState(FireBossState.Idle);
    }
}
