using System.Collections;
using UnityEngine;

/// <summary>
/// Componente que dispara una semilla al jugador.
/// </summary>
public class ExplodingSeedAttack : MonoBehaviour
{
    [Header("Configuración Semilla")]
    [SerializeField] private GameObject seedPrefab;    // Prefab de la semilla
    [SerializeField] private Transform shootPoint;     // Punto de disparo
    [SerializeField] private float seedSpeed = 12f;    // Velocidad de la semilla
    [SerializeField] private float seedLifetime = 5f;  // Tiempo antes de explotar

    [Header("Configuración Explosión")]
    [SerializeField] private GameObject explosionVFX;      // VFX de explosión
    [SerializeField] private float explosionVFXDuration = 1f;

    [Header("Configuración Pinchos")]
    [SerializeField] private GameObject spikePrefab;   // Prefab del pincho vegetal
    [SerializeField] private int spikeCount = 8;       // Número de pinchos
    [SerializeField] private float spikeSpeed = 8f;    // Velocidad de los pinchos
    [SerializeField] private float spikeLifetime = 2f; // Vida útil de los pinchos

    private Transform playerTarget;

    private void Awake()
    {
        var playerGo = GameObject.FindGameObjectWithTag("player");
        if (playerGo != null)
            playerTarget = playerGo.transform;
    }

    /// <summary>
    /// Dispara la semilla hacia el jugador.
    /// </summary>
    public void LaunchAttack()
    {
        if (seedPrefab == null || shootPoint == null || playerTarget == null)
        {
            Debug.LogWarning("ExplodingSeedAttack: falta prefab, punto de disparo o target");
            return;
        }

        // Calcula dirección y rotación
        Vector3 spawnPos = shootPoint.position;
        Vector2 dir = (playerTarget.position - spawnPos).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // Instancia y orienta la semilla
        var seed = Instantiate(
            seedPrefab,
            spawnPos,
            Quaternion.Euler(0f, 0f, angle - 90f)
        );

        // Aplica velocidad
        var rb = seed.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("ExplodingSeedAttack: el prefab de semilla no tiene Rigidbody2D");
            Destroy(seed);
            return;
        }
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0f;
        rb.velocity = dir * seedSpeed;

        // Añade comportamiento explosivo
        var expl = seed.AddComponent<ExplodingSeed>();
        expl.Initialize(
            spikePrefab, spikeCount, spikeSpeed, spikeLifetime,
            explosionVFX, explosionVFXDuration
        );
        expl.InitiateLifetime(seedLifetime);
    }
}

/// <summary>
/// Comportamiento de la semilla explosiva.
/// Explota al colisionar o al expirar su tiempo, generando pinchos radialmente.
/// </summary>
public class ExplodingSeed : MonoBehaviour
{
    private GameObject spikePrefab;
    private int spikeCount;
    private float spikeSpeed;
    private float spikeLifetime;
    private GameObject explosionVFX;
    private float explosionVFXDuration;
    private bool exploded = false;

    /// <summary>
    /// Inicializa parámetros de explosión.
    /// </summary>
    public void Initialize(
        GameObject spikePrefab,
        int count,
        float speed,
        float life,
        GameObject vfx,
        float vfxDuration)
    {
        this.spikePrefab = spikePrefab;
        this.spikeCount = count;
        this.spikeSpeed = speed;
        this.spikeLifetime = life;
        this.explosionVFX = vfx;
        this.explosionVFXDuration = vfxDuration;
    }

    /// <summary>
    /// Programa la explosión tras un tiempo si no colisiona antes.
    /// </summary>
    public void InitiateLifetime(float lifetime)
    {
        StartCoroutine(LifetimeCoroutine(lifetime));
    }

    private IEnumerator LifetimeCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        Explode();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (exploded) return;
        var other = col.gameObject;

        // Condiciones de explosión
        if (other.layer == LayerMask.NameToLayer("Suelo")
            || other.CompareTag("player")
            || other.CompareTag("Wall"))
        {
            Explode();
        }
    }

    /// <summary>
    /// Ejecuta la explosión y genera pinchos radialmente.
    /// </summary>
    private void Explode()
    {
        if (exploded) return;
        exploded = true;

        // VFX de explosión
        if (explosionVFX != null)
        {
            var vfx = Instantiate(explosionVFX, transform.position, Quaternion.identity);
            Destroy(vfx, explosionVFXDuration);
        }

        // Genera pinchos en círculo
        float angleStep = 360f / spikeCount;
        for (int i = 0; i < spikeCount; i++)
        {
            float angle = i * angleStep;
            Vector3 dir = new Vector3(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad),
                0f
            ).normalized;

            var spike = Instantiate(
                spikePrefab,
                transform.position,
                Quaternion.FromToRotation(Vector3.up, dir)
            );
            var rb = spike.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.gravityScale = 0f;
                rb.velocity = dir * spikeSpeed;
            }
            Destroy(spike, spikeLifetime);
        }

        // Elimina la semilla
        Destroy(gameObject);
    }
}
