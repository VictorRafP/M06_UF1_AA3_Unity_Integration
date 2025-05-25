using System.Collections;
using UnityEngine;

[System.Serializable]
public class CollectPhaseData
{
    public GameObject[] platforms;
    public GameObject[] gems;
}

public class BossFaseManager : MonoBehaviour
{
    [Header("Configuración de fases (0 = fase 2, 1 = fase 3, ...)")]
    public CollectPhaseData[] phases;

    [Header("Fondo")]
    public GameObject FondoFase;
    public GameObject OjosFase;
    public GameObject CristalObject;
    public Animator FondoAnimator;
    public Animator OjosAnimator;
    public Animator CristalAnimator;

    [Header("Retraso tras completar recolección")]
    public float postCollectDelay = 1f;

    public BoxCollider2D bounceCollider;
    public GameObject shieldSprite;

    [Header("Arrastra aquí el GameObject de tu Boss (Fire, Plant o Rayo)")]
    public GameObject bossObject;
    public Animator bossAnimator;

    public int _currentPhase = -1;
    public int _gemsRemaining;
    public bool fondoPuesto = false;
    public bool cristalactive = false;
    public bool BossAcabado = false;

    public void StartCollectPhase(int phaseIndex)
    {
        EndCollectPhaseImmediate();

        _currentPhase = phaseIndex;
        if (phaseIndex < 0 || phaseIndex >= phases.Length) return;

        if (bossObject != null)
        {
            var sr = bossObject.GetComponent<SpriteRenderer>();
            if (sr != null) sr.enabled = false;

            var col = bossObject.GetComponent<Collider2D>();
            if (col != null) col.enabled = false;

            bounceCollider.enabled = false;

            shieldSprite.SetActive(false);
        }
        if (!cristalactive)
        {
            foreach (var p in phases[phaseIndex].platforms)
                p.SetActive(true);

            var gems = phases[phaseIndex].gems;
            _gemsRemaining = gems.Length;
            foreach (var g in gems)
            {

                g.SetActive(true);
                var ci = g.GetComponent<CollectibleItem>();
                if (ci != null) ci.Initialize(this);
            }
            cristalactive = true;
    }

        if (!fondoPuesto)
        {
            GameObject FondoInst = Instantiate(FondoFase, new Vector3(0f, 0f, 0f), Quaternion.identity);
            GameObject OjosInst = Instantiate(OjosFase, new Vector3(0f, 0f, 0f), Quaternion.identity);
            GameObject CristalInst = Instantiate(CristalObject, new Vector3(0f, 0f, 0f), Quaternion.identity);

            FondoAnimator = FondoInst.GetComponent<Animator>();
            OjosAnimator = OjosInst.GetComponent<Animator>();
            CristalAnimator = CristalInst.GetComponent<Animator>();
            fondoPuesto = true;
        }
    }

    public void OnGemCollected()
    {
        _gemsRemaining--;
        if (_gemsRemaining <= 0)
        {
            if (_currentPhase != 2)
            {
                FondoAnimator.SetTrigger("Delete");
                OjosAnimator.SetTrigger("Delete");
                CristalAnimator.SetTrigger("Delete");
                fondoPuesto = false;
                cristalactive = false;
            }
            else
            {
                OjosAnimator.SetTrigger("Romper");
                StartCoroutine(DelaycambioEscena());
            }

                StartCoroutine(EndCollectPhase());
        }
        
    }

    IEnumerator DelaycambioEscena()
    {
        yield return new WaitForSeconds(2f);
        BossAcabado = true;
    }

    IEnumerator EndCollectPhase()
    {
        yield return new WaitForSeconds(postCollectDelay);

        EndCollectPhaseImmediate();
        if (_currentPhase != 2)
        {
            if (bossObject != null)
            {
                var sr = bossObject.GetComponent<SpriteRenderer>();
                if (sr != null) sr.enabled = true;

                var col = bossObject.GetComponent<Collider2D>();
                if (col != null) col.enabled = true;

                bounceCollider.enabled = true;
                bossAnimator.SetTrigger("Aparicion");
                shieldSprite.SetActive(true);
            }

            bossObject?.SendMessage(
                "OnCollectPhaseEnded",
                SendMessageOptions.DontRequireReceiver
            );
        }
    }

    void EndCollectPhaseImmediate()
    {
        if (_currentPhase < 0 || _currentPhase == 2) return;

        foreach (var p in phases[_currentPhase].platforms)
            p.SetActive(false);
        foreach (var g in phases[_currentPhase].gems)
            g.SetActive(false);

        _currentPhase = -1;
    }
}
