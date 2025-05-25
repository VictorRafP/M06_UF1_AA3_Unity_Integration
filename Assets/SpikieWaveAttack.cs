using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum SpikePattern { Forward, Backward, FromEdges }

public class SpikieWaveAttack : MonoBehaviour
{
    [Header("Puntos de warning (0-11)")]
    [SerializeField] private Transform[] warningPoints = new Transform[12];

    [Header("Prefabs")]
    [SerializeField] private GameObject warningPrefab;
    [SerializeField] private GameObject spikePrefab;

    [Header("Tiempos")]
    [SerializeField] private float warningDuration = 1f;
    [SerializeField] private float warningInterval = 0.2f;
    [SerializeField] private float spikeSpeed = 10f;
    [SerializeField] private float spikeLifetime = 3f;
    [SerializeField] private float spikeInterval = 0.1f;

    [Header("Offset de aparición del pincho")]
    [SerializeField] private float spawnOffset = 3f;

    public void LaunchAttack()
    {
        var pattern = (SpikePattern)Random.Range(0, 3);
        StartCoroutine(VolleyCoroutine(pattern));
    }

    private IEnumerator VolleyCoroutine(SpikePattern pattern)
    {
        if (warningPoints.Length != 12)
        {
            Debug.LogError("SpikieWaveAttack: debes asignar exactamente 12 warningPoints.");
            yield break;
        }

        List<int[]> groups = new List<int[]>();
        switch (pattern)
        {
            case SpikePattern.Forward:
                for (int i = 0; i < 6; i++) groups.Add(new int[] { i });
                break;
            case SpikePattern.Backward:
                for (int i = 11; i >= 6; i--) groups.Add(new int[] { i });
                break;
            case SpikePattern.FromEdges:
                groups.Add(new int[] { 5, 0 });
                groups.Add(new int[] { 4, 1 });
                groups.Add(new int[] { 3, 2 });
                break;
        }

        var warningGroups = new List<GameObject[]>();
        foreach (var grp in groups)
        {
            var warns = new GameObject[grp.Length];
            for (int j = 0; j < grp.Length; j++)
                warns[j] = Instantiate(warningPrefab, warningPoints[grp[j]].position, Quaternion.identity);
            AudioManager.Instance.PlaySFXFromEnum(AudioManager.SFX_Sounds.PLANT_BOSS);
            warningGroups.Add(warns);
            yield return new WaitForSeconds(warningInterval);
        }

        yield return new WaitForSeconds(warningDuration);

        for (int g = 0; g < groups.Count; g++)
        {
            var grp = groups[g];
            foreach (var w in warningGroups[g])
                if (w != null) Destroy(w);

            foreach (int idx in grp)
            {
                Vector3 finalPos = warningPoints[idx].position;
                Vector3 startPos = finalPos + Vector3.down * spawnOffset;

                var spikeGO = Instantiate(spikePrefab, startPos, Quaternion.identity);
                AudioManager.Instance.PlaySFXFromEnum(AudioManager.SFX_Sounds.PLANT_BOSS_CHARGE);
                spikeGO.transform.rotation = Quaternion.FromToRotation(Vector3.up, Vector3.up);

                StartCoroutine(SpikeLifecycle(spikeGO.transform, startPos, finalPos));
            }

            yield return new WaitForSeconds(spikeInterval);
        }
    }

    private IEnumerator SpikeLifecycle(Transform spike, Vector3 start, Vector3 end)
    {
        float dist = Vector3.Distance(start, end);
        float tPop = 0f;
        float durPop = dist / spikeSpeed;
        while (tPop < durPop)
        {
            spike.position = Vector3.Lerp(start, end, tPop / durPop);
            tPop += Time.deltaTime;
            yield return null;
        }
        spike.position = end;

        yield return new WaitForSeconds(spikeLifetime);

        float tRet = 0f;
        while (tRet < durPop)
        {
            spike.position = Vector3.Lerp(end, start, tRet / durPop);
            tRet += Time.deltaTime;
            yield return null;
        }
        spike.position = start;

        Destroy(spike.gameObject);
    }
}
