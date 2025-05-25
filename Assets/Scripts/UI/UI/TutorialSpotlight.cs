using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSpotlight : MonoBehaviour
{
    [Header("Zonas a resaltar (en orden)")]
    public RectTransform[] zones;

    [Header("Textos para cada zona (en orden)")]
    public GameObject[] zoneTexts;

    [Header("Panel de Overlay (con Material personalizado)")]
    public Image overlayImage;

    [Header("Cámara para zoom (debe ser ortográfica)")]
    public Camera mainCamera;

    [Header("Parámetros de Animación")]
    public float spotlightTransitionDuration = 0.5f;
    public float zoomTransitionDuration = 0.5f;
    public float targetZoomSize = 5f;

    private Material overlayMaterial;
    private RectTransform canvasRectTransform;
    private int currentZoneIndex = 0;
    private float originalCameraSize;

    // Variable para saber si el tutorial está activo
    public static bool tutorialActive = false;

    void Start()
    {
        // Validaciones básicas
        if (overlayImage == null || zones == null || zoneTexts == null ||
            zones.Length == 0 || zoneTexts.Length != zones.Length)
        {
            Debug.LogError("TutorialSpotlight: configuración incompleta");
            enabled = false;
            return;
        }

        // Pausar el juego y bloquear inputs
        Time.timeScale = 0f;
        tutorialActive = true;

        // Preparar overlay
        overlayMaterial = Instantiate(overlayImage.material);
        overlayImage.material = overlayMaterial;

        // Obtener rectTransform del canvas
        var parentCanvas = overlayImage.GetComponentInParent<Canvas>();
        canvasRectTransform = parentCanvas?.GetComponent<RectTransform>();

        // Guardar tamaño de cámara
        originalCameraSize = mainCamera != null ? mainCamera.orthographicSize : 0f;

        // Desactivar todos los textos
        foreach (var txt in zoneTexts) txt.SetActive(false);

        StartCoroutine(TutorialSequence());
    }

    IEnumerator TutorialSequence()
    {
        while (currentZoneIndex < zones.Length)
        {
            yield return StartCoroutine(AnimateSpotlightToZone(zones[currentZoneIndex]));

            if (mainCamera != null)
            {
                yield return StartCoroutine(AnimateCameraZoom(targetZoomSize));
            }

            zoneTexts[currentZoneIndex].SetActive(true);

            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

            zoneTexts[currentZoneIndex].SetActive(false);

            if (mainCamera != null && currentZoneIndex < zones.Length - 1)
            {
                yield return StartCoroutine(AnimateCameraZoom(originalCameraSize));
            }

            currentZoneIndex++;
        }

        overlayImage.gameObject.SetActive(false);

        // Restaurar el tiempo y los inputs al finalizar el tutorial
        Time.timeScale = 1f;
        tutorialActive = false;  // El tutorial ha terminado
    }

    IEnumerator AnimateSpotlightToZone(RectTransform targetZone)
    {
        Vector4 startCenter = overlayMaterial.GetVector("_RectCenter");
        Vector4 startSize = overlayMaterial.GetVector("_RectSize");

        Vector4 targetCenter, targetSize;
        ComputeZoneUV(targetZone, out targetCenter, out targetSize);

        float elapsed = 0f;
        while (elapsed < spotlightTransitionDuration)
        {
            float t = elapsed / spotlightTransitionDuration;
            overlayMaterial.SetVector("_RectCenter", Vector4.Lerp(startCenter, targetCenter, t));
            overlayMaterial.SetVector("_RectSize", Vector4.Lerp(startSize, targetSize, t));
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        overlayMaterial.SetVector("_RectCenter", targetCenter);
        overlayMaterial.SetVector("_RectSize", targetSize);
    }

    IEnumerator AnimateCameraZoom(float targetSize)
    {
        float startSize = mainCamera.orthographicSize;
        float elapsed = 0f;
        while (elapsed < zoomTransitionDuration)
        {
            float t = elapsed / zoomTransitionDuration;
            mainCamera.orthographicSize = Mathf.Lerp(startSize, targetSize, t);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        mainCamera.orthographicSize = targetSize;
    }

    void ComputeZoneUV(RectTransform targetZone, out Vector4 centerUV, out Vector4 sizeUV)
    {
        Vector3[] corners = new Vector3[4];
        targetZone.GetWorldCorners(corners);
        Vector3 bottomLeft = corners[0];
        Vector3 topRight = corners[2];

        Vector2 localBL, localTR;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRectTransform,
            RectTransformUtility.WorldToScreenPoint(null, bottomLeft),
            null,
            out localBL);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRectTransform,
            RectTransformUtility.WorldToScreenPoint(null, topRight),
            null,
            out localTR);

        Rect canvasRect = canvasRectTransform.rect;

        Vector2 blUV = new Vector2(
            (localBL.x - canvasRect.x) / canvasRect.width,
            (localBL.y - canvasRect.y) / canvasRect.height
        );
        Vector2 trUV = new Vector2(
            (localTR.x - canvasRect.x) / canvasRect.width,
            (localTR.y - canvasRect.y) / canvasRect.height
        );

        Vector2 center = (blUV + trUV) * 0.5f;
        Vector2 size = trUV - blUV;
        centerUV = new Vector4(center.x, center.y, 0, 0);
        sizeUV = new Vector4(size.x, size.y, 0, 0);
    }
}
