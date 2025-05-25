using System.Collections;
using UnityEngine;

public class ShockwaveAttack : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private GameObject shockwavePrefab;
    [SerializeField] private float shockwaveSpeed = 5f;

    [Header("Camera Shake")]
    [SerializeField] private float cameraShakeIntensity = 0.1f;
    [SerializeField] private float cameraShakeDuration = 0.3f;

    private Camera mainCamera;
    private Vector3 originalLocalPos;

    private void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("ShockwaveAttack: no se encontró Main Camera en escena.");
            return;
        }
        originalLocalPos = mainCamera.transform.localPosition;
    }

    public void LaunchAttack()
    {
        GameObject shockwave = Instantiate(shockwavePrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = shockwave.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.right * shockwaveSpeed;
        }
        Destroy(shockwave, 3f);

        StartCoroutine(CameraShake());
        AudioManager.Instance.PlaySFXFromEnum(AudioManager.SFX_Sounds.PLANT_BOSS);
    }

    private IEnumerator CameraShake()
    {
        float elapsed = 0f;
        while (elapsed < cameraShakeDuration)
        {
            elapsed += Time.deltaTime;
            Vector2 offset = Random.insideUnitCircle * cameraShakeIntensity;
            mainCamera.transform.localPosition = originalLocalPos + new Vector3(offset.x, offset.y, 0f);
            yield return null;
        }
        mainCamera.transform.localPosition = originalLocalPos;
    }
}
