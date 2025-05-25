using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorRainAttack : MonoBehaviour
{
    [Header("Spawn Warning")]
    [SerializeField] private BoxCollider2D spawnArea;

    [Header("Altura de aparición")]
    [SerializeField] private float spawnHeightOffset = 10f;

    [Header("Prefabs")]
    [SerializeField] private GameObject warningPrefab;
    [SerializeField] private GameObject rockPrefab;

    [Header("Configuración")]
    [SerializeField] private int rockCount = 8;
    [SerializeField] private float warningDuration = 1.2f;
    [SerializeField] private float rockFallSpeed = 20f;
    [SerializeField] private float rockLifetime = 5f;

    [Header("Separación mínima entre instancias (unidades X)")]
    [SerializeField] private float minSeparation = 2f;

    public void LaunchAttack()
    {
        StartCoroutine(SpawnMeteorShower());
    }

    private IEnumerator SpawnMeteorShower()
    {
        var positions = new List<Vector2>();

        for (int i = 0; i < rockCount; i++)
        {
            Vector2 groundPos;
            int safety = 0;
            do
            {
                float x = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x);
                groundPos = new Vector2(x, spawnArea.bounds.min.y);
                safety++;
                if (safety > 20) break; 
            }
            while (IsTooClose(groundPos, positions, minSeparation));

            positions.Add(groundPos);

            GameObject warn = Instantiate(warningPrefab, groundPos, Quaternion.identity);
            Destroy(warn, warningDuration);

            yield return null; 
        }
        AudioManager.Instance.PlaySFXFromEnum(AudioManager.SFX_Sounds.FIRE_BOSS_CHARGE);

        yield return new WaitForSecondsRealtime(warningDuration);

        Time.timeScale = 1f;
        yield return null;

        foreach (var groundPos in positions)
        {
            Vector2 spawnPos = new Vector2(
                groundPos.x,
                spawnArea.bounds.min.y + spawnHeightOffset
            );

            GameObject rock = Instantiate(rockPrefab, spawnPos, Quaternion.identity);
            var col = rock.GetComponent<Collider2D>();
            if (col != null) col.enabled = false;

            var rb = rock.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.gravityScale = 2f;
                rb.velocity = Vector2.down * rockFallSpeed;
            }

            StartCoroutine(EnableColliderAfter(col, 0.05f));

            Destroy(rock, rockLifetime);
            yield return null;
        }
        AudioManager.Instance.PlaySFXFromEnum(AudioManager.SFX_Sounds.FIRE_BOSS);
    }

    private bool IsTooClose(Vector2 pos, List<Vector2> others, float minSep)
    {
        foreach (var o in others)
            if (Mathf.Abs(o.x - pos.x) < minSep)
                return true;
        return false;
    }

    private IEnumerator EnableColliderAfter(Collider2D col, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (col != null) col.enabled = true;
    }
}
