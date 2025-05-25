using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderFollow : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private GameObject ThunderPrefab;
    [SerializeField] private GameObject ThunderAlertPrefab;
    [SerializeField] private float cameraShakeIntensity = 0.2f;
    [SerializeField] private float cameraShakeDuration = 0.3f;
    [SerializeField] private int thunderQuantity = 6;
    [SerializeField] private float thunderOffset = 2f;

    private Camera mainCamera;
    private Vector3 originalLocalPos;

    private void EnsureCameraInitialized()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera != null)
            {
                originalLocalPos = mainCamera.transform.localPosition;
            }
            else
            {
                Debug.LogError("No se encontró la cámara principal (Camera.main)");
            }
        }
    }

    public void LaunchAttack()
    {
        EnsureCameraInitialized();
        thunderOffset = 2;
        StartCoroutine(ThunderLaunch());
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


    private IEnumerator ThunderLaunch()
    {
        for (int i = 0; i < thunderQuantity; i++)
        {
            GameObject thunderAlert = Instantiate(ThunderAlertPrefab, new Vector3(transform.position.x + 1 + thunderOffset, -2 , transform.position.z), Quaternion.identity);
            AudioManager.Instance.PlaySFXFromEnum(AudioManager.SFX_Sounds.LIGHTNING_BOSS_BALL_CHARGE);
            Destroy(thunderAlert, 1f);
            yield return new WaitForSeconds(1f);
            GameObject thunder = Instantiate(ThunderPrefab, new Vector3(transform.position.x + 1 + thunderOffset, transform.position.y + 8.5f, transform.position.z), Quaternion.identity);
            thunder.transform.localScale *= .65f;
            StartCoroutine(CameraShake());
            thunderOffset += 2;
            AudioManager.Instance.PlaySFXFromEnum(AudioManager.SFX_Sounds.LIGHTNING_BOSS);
            Destroy(thunder, 1f);
            yield return new WaitForSeconds(1f);
        }
    }
}
