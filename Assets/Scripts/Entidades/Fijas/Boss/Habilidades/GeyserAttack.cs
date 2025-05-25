using System.Collections;
using UnityEngine;

public class GeyserAttack : MonoBehaviour
{
    [Header("Spawn Area")]
    [SerializeField] private BoxCollider2D spawnArea;

    [Header("Prefabs")]
    [SerializeField] private GameObject warningPrefab;
    [SerializeField] private GameObject geyserPrefab;

    [Header("Timings")]
    [SerializeField] private float warningDuration = 1.5f;
    [SerializeField] private float growthTime = 0.5f;
    [SerializeField] private float geyserLifetime = 2f;

    [Header("Scale")]
    [SerializeField] private float minHeight = 0f;
    [SerializeField] private float maxHeight = 3f;

    public void LaunchAttack()
    {
        Vector2 pos = new Vector2(
            Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x),
            spawnArea.bounds.min.y
        );
        StartCoroutine(SpawnGeyserSequence(pos));
    }

    private IEnumerator SpawnGeyserSequence(Vector2 position)
    {
        GameObject warning = Instantiate(warningPrefab, position, Quaternion.identity);
        AudioManager.Instance.PlaySFXFromEnum(AudioManager.SFX_Sounds.FIRE_BOSS_CHARGE);
        Destroy(warning, warningDuration);

        yield return new WaitForSeconds(warningDuration);

        GameObject geyser = Instantiate(geyserPrefab, position, Quaternion.identity);
        geyser.transform.localScale = new Vector3(1f, minHeight, 1f);
        AudioManager.Instance.PlaySFXFromEnum(AudioManager.SFX_Sounds.FIRE_BOSS);

        float elapsed = 0f;
        while (elapsed < growthTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / growthTime);
            float height = Mathf.Lerp(minHeight, maxHeight, t);
            geyser.transform.localScale = new Vector3(1f, height, 1f);
            yield return null;
        }

        yield return new WaitForSeconds(geyserLifetime);
        Destroy(geyser);
    }
}
