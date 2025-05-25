using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeAttack : MonoBehaviour
{
    [Header("Configuración de Pinchos")]
    [SerializeField] private int spikeCount = 3;
    [SerializeField] private GameObject warningPrefab;
    [SerializeField] private GameObject spikePrefab;

    [Header("Puntos de Spawn")]
    [SerializeField] private Transform[] groundSpawns;
    [SerializeField] private Transform[] sideSpawns;

    [Header("Ajustes de Timing y Velocidad")]
    [SerializeField] private float warningDuration = 1f;
    [SerializeField] private float spikeSpeed = 20f;
    [SerializeField] private float spikeLifetime = 2f;

    [Header("Offsets de Inicio")]
    [SerializeField] private float groundOffset = 3f;
    [SerializeField] private float sideOffset = 3f;

    private List<int> lastUsedIndices = new List<int>();
    private int attackCounter = 0;

    public void LaunchAttack()
    {
        StartCoroutine(SpawnSpikes());
    }

    private IEnumerator SpawnSpikes()
    {
        var allSpawns = new List<Transform>();
        allSpawns.AddRange(groundSpawns);
        allSpawns.AddRange(sideSpawns);

        int totalSpawns = allSpawns.Count;

        if (attackCounter >= 2)
        {
            lastUsedIndices.Clear();
            attackCounter = 0;
        }

        var availableIndices = new List<int>();
        for (int i = 0; i < totalSpawns; i++)
            if (!lastUsedIndices.Contains(i))
                availableIndices.Add(i);

        var chosen = new List<Transform>();
        var chosenIndices = new List<int>();
        for (int i = 0; i < spikeCount && availableIndices.Count > 0; i++)
        {
            int pick = Random.Range(0, availableIndices.Count);
            int idx = availableIndices[pick];
            availableIndices.RemoveAt(pick);
            chosenIndices.Add(idx);
            chosen.Add(allSpawns[idx]);
        }
        lastUsedIndices = new List<int>(chosenIndices);
        attackCounter++;

        foreach (var spawnPoint in chosen)
        {
            var warn = Instantiate(warningPrefab, spawnPoint.position, Quaternion.identity);
            AudioManager.Instance.PlaySFXFromEnum(AudioManager.SFX_Sounds.PLANT_BOSS);
            Vector3 dirWarn = System.Array.Exists(groundSpawns, t => t == spawnPoint)
                              ? Vector3.up : Vector3.left;
            warn.transform.rotation = Quaternion.FromToRotation(Vector3.up, dirWarn);
            Destroy(warn, warningDuration);
        }

        yield return new WaitForSecondsRealtime(warningDuration);

        foreach (var spawnPoint in chosen)
        {
            Vector3 finalPos = spawnPoint.position;
            Vector3 startPos = finalPos;
            Vector3 dir;

            if (System.Array.Exists(groundSpawns, t => t == spawnPoint))
            {
                startPos.y -= groundOffset;
                dir = Vector3.up;
            }
            else
            {
                startPos.x += sideOffset;
                dir = Vector3.left;
            }

            var spike = Instantiate(spikePrefab, startPos, Quaternion.identity);
            AudioManager.Instance.PlaySFXFromEnum(AudioManager.SFX_Sounds.PLANT_BOSS_CHARGE);
            spike.transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);

            StartCoroutine(SpikeLifecycle(spike.transform, startPos, finalPos));
        }
    }

    private IEnumerator SpikeLifecycle(Transform t, Vector3 start, Vector3 end)
    {
        float dist = Vector3.Distance(start, end);
        float durationUp = dist / spikeSpeed;
        float elapsed = 0f;
        while (elapsed < durationUp)
        {
            t.position = Vector3.Lerp(start, end, elapsed / durationUp);
            elapsed += Time.deltaTime;
            yield return null;
        }
        t.position = end;

        yield return new WaitForSeconds(spikeLifetime);

        elapsed = 0f;
        while (elapsed < durationUp)
        {
            t.position = Vector3.Lerp(end, start, elapsed / durationUp);
            elapsed += Time.deltaTime;
            yield return null;
        }
        t.position = start;

        Destroy(t.gameObject);
    }
}
