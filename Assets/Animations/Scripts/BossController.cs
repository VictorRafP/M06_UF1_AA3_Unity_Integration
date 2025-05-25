using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum BossState { Idle, Attack1, Attack2, Attack3, Death }

public class BossController : MonoBehaviour
{
    public float winTimeCelebration = 10000f;
    public float endWinCelebration = 0;
    public bool ultimoBoss = false;

    public VidaEnemigo tipo;

    [Header("Tiempo antes de Fase")]
    [SerializeField] private float initialDelay = 2f;
    [SerializeField] private float transitionDelay = 1f;

    public BossState currentState = BossState.Idle;

    [System.Serializable]
    public class BossPhase
    {
        public string phaseName;
        public float[] attackProbabilities; // Attack1, Attack2, Attack3
    }

    [SerializeField] private BossPhase[] phases;
    [SerializeField] private HP_Boss hpBoss;

    public Animator animator;
    private int currentPhaseIndex = 0;

    [Header("Habilidades")]
    [SerializeField] private FireballAttack Attack1;
    [SerializeField] private ShockwaveAttack Attack2;
    [SerializeField] private GeyserAttack Attack3;

    // Duración de cada ataque
    [Header("Duración de ataques")]
    public float durationAttack1 = 2f;
    public float durationAttack2 = 2.5f;
    public float durationAttack3 = 3f;

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
        ChangeState(BossState.Idle);
    }

    void Update()
    {
        if (hpBoss != null)
        {
            CheckBoss(); // Verifica vida y fases
        }

    }

    // MÁQUINA DE ESTADOS
    public void ChangeState(BossState newState)
    {
        if (currentState == BossState.Death) return;

        currentState = newState;
        Debug.Log("Estado actual: " + currentState);

        switch (currentState)
        {
            case BossState.Idle:
                //animator.SetTrigger("Idle");
                ChooseAttack();
                break;
            case BossState.Attack1:
                //animator.SetTrigger("Attack1");
                Attack1.LaunchAttack();
                StartCoroutine(ExecuteAttack(BossState.Attack1, durationAttack1));
                break;
            case BossState.Attack2:
                //animator.SetTrigger("Attack2");
                Attack2.LaunchAttack();
                StartCoroutine(ExecuteAttack(BossState.Attack2, durationAttack2));
                break;
            case BossState.Attack3:
                //animator.SetTrigger("Attack3");
                Attack3.LaunchAttack();
                StartCoroutine(ExecuteAttack(BossState.Attack3, durationAttack3));
                break;
            case BossState.Death:
                //animator.SetTrigger("Death");
                Destroy(gameObject);
                break;
        }
    }

    private void ChooseAttack()
    {
        BossPhase currentPhase = phases[currentPhaseIndex];
        List<BossState> validAttacks = new List<BossState>();

        for (int i = 0; i < currentPhase.attackProbabilities.Length; i++)
        {
            if (currentPhase.attackProbabilities[i] > 0)
            {
                validAttacks.Add(BossState.Attack1 + i);
            }
        }

        if (validAttacks.Count == 0) return;

        int attackIndex = Choose(currentPhase.attackProbabilities);
        BossState chosenAttack = validAttacks[attackIndex];
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

    private IEnumerator ExecuteAttack(BossState attackState, float duration)
    {
        yield return new WaitForSeconds(duration);

        // animator.SetTrigger("PhaseTransition");

        yield return new WaitForSeconds(transitionDelay);

        ChangeState(BossState.Idle);
    }

    private void CheckBoss()
    {
        if (hpBoss.GetCurrentHealth() <= hpBoss.GetMaxHealth() * 0.5f && currentPhaseIndex == 0)
        {
            currentPhaseIndex = 1;
            Debug.Log("¡Fase 2 activada!");
        }

        if (hpBoss.GetCurrentHealth() <= 0)
        {
            ChangeState(BossState.Death);
            GameManager.Instance.LoadNextScene();
        }
    }
}
