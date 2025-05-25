using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballAttack : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float fireballSpeed = 7f;
    [SerializeField] private int fireballCount = 3;
    [SerializeField] private float timeBetweenShots = 0.3f;
    [SerializeField] private float projectileLifetime = 4f;

    [Header("Ajuste del angulo de trayectoria")]
    [Tooltip("Grados que se suman al angulo base (positivo sube, negativo baja)")]
    [SerializeField] private float angleOffset = 15f;
    [Tooltip("Para abanico de bolas, poner > 0")]
    [SerializeField] private float spreadAngle = 10f;

    private Transform playerTarget;

    private void Awake()
    {
        var playerGo = GameObject.FindGameObjectWithTag("player");
        if (playerGo != null)
            playerTarget = playerGo.transform;
    }

    public void LaunchAttack()
    {
        if (playerTarget == null) return;
        StartCoroutine(ShootFireballs());
    }

    private IEnumerator ShootFireballs()
    {
        Vector2 dir = (playerTarget.position - shootPoint.position).normalized;
        float baseAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        float startAngle = baseAngle + angleOffset - spreadAngle * 0.5f;
        float step = spreadAngle / (fireballCount - 1);

        for (int i = 0; i < fireballCount; i++)
        {
            float angle = startAngle + step * i;
            var proj = Instantiate(fireballPrefab, shootPoint.position, Quaternion.Euler(0, 0, angle - 90f));

            var rb = proj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 v = new Vector2(
                    Mathf.Cos(angle * Mathf.Deg2Rad),
                    Mathf.Sin(angle * Mathf.Deg2Rad)
                ).normalized;
                rb.velocity = v * fireballSpeed;
            }

            Destroy(proj, projectileLifetime);
            AudioManager.Instance.PlaySFXFromEnum(AudioManager.SFX_Sounds.FIRE_BOSS);

            yield return new WaitForSeconds(timeBetweenShots);
        }
    }
}
