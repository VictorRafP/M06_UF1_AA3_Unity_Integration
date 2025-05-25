using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HitFeedbackController : MonoBehaviour
{
    [Header("Freeze Time")]
    public float freezeDuration = 0.05f;
    public float freezeRampup = 0.1f;

    [Header("Camera Punch")]
    public float punchFactor = 1.1f;
    public float punchDuration = 0.2f;
    private float originalCamSize;

    [Header("Damage Overlay")]
    public Image damageOverlay;
    public Color overlayColor = new Color(1, 0, 0, 0.6f);
    public float overlayFadeTime = 0.3f;

    void Awake()
    {
        originalCamSize = Camera.main.orthographicSize;
        damageOverlay.color = new Color(overlayColor.r, overlayColor.g, overlayColor.b, 0f);
    }

    public void PlayHitFeedback()
    {
        var player = GameObject.FindWithTag("Player")?.GetComponent<PlayerBoss>();
        if (player != null && player.isInvincible)
            return;

        StopAllCoroutines();
        StartCoroutine(FreezeTime());
        StartCoroutine(CameraPunch());
        StartCoroutine(OverlayFlash());
    }

    private IEnumerator FreezeTime()
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(freezeDuration);
        float t = 0f;
        while (t < freezeRampup)
        {
            t += Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Lerp(0f, 1f, t / freezeRampup);
            yield return null;
        }
        Time.timeScale = 1f;
    }

    private IEnumerator CameraPunch()
    {
        float half = punchDuration * 0.5f;
        float t = 0f;
        float targetSize = originalCamSize / punchFactor;

        // Zoom in
        while (t < half)
        {
            t += Time.deltaTime;
            Camera.main.orthographicSize = Mathf.Lerp(originalCamSize, targetSize, t / half);
            yield return null;
        }
        // Zoom out
        while (t < punchDuration)
        {
            t += Time.deltaTime;
            Camera.main.orthographicSize = Mathf.Lerp(targetSize, originalCamSize, (t - half) / half);
            yield return null;
        }
        Camera.main.orthographicSize = originalCamSize;
    }

    private IEnumerator OverlayFlash()
    {
        damageOverlay.color = overlayColor;
        float t = 0f;
        while (t < overlayFadeTime)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(overlayColor.a, 0f, t / overlayFadeTime);
            damageOverlay.color = new Color(overlayColor.r, overlayColor.g, overlayColor.b, a);
            yield return null;
        }
        damageOverlay.color = new Color(overlayColor.r, overlayColor.g, overlayColor.b, 0f);
    }
}
