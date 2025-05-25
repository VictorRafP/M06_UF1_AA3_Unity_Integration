using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [ColorUsage(true, true)]
    [SerializeField] private Color _flashColor = Color.white;
    [SerializeField] private float _flashTime = 0.15f;
    [SerializeField] private AnimationCurve _flashSpeedCurve;

    private SpriteRenderer[] _spriteRenderers;
    private Material[] _materials;

    private Coroutine _damageFlashCoroutine;

    private void Awake()
    {
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        Init();
    }

    public void Init()
    {
        _materials = new Material[_spriteRenderers.Length];
        for (int i = 0; i < _spriteRenderers.Length; i++)
        {
            _materials[i] = _spriteRenderers[i].material;
        }
    }

    public void CallDamageFlash()
    {
        _damageFlashCoroutine = StartCoroutine(DamageFlasher());
    }

    private IEnumerator DamageFlasher()
    {
        // set the color
        SetFlashColor();

        float currentFlashAmount = 0f;
        float elapsedTime = 0f;

        while (elapsedTime < _flashTime)
        {
            // iterate elapsedTime
            elapsedTime += Time.deltaTime;

            // lerp the flash amount
            currentFlashAmount = Mathf.Lerp(1f, _flashSpeedCurve.Evaluate(elapsedTime), (elapsedTime / _flashTime));
            SetFlashAmount(currentFlashAmount);

            yield return null;
        }
    }

    // Start is called before the first frame update
    private void SetFlashColor()
    {
        // set the color
        for (int i = 0; i < _materials.Length; i++)
        {
            _materials[i].SetColor("_FlashColor", _flashColor);
        }
    }

    private void SetFlashAmount(float amount)
    {
        // set the flash amount
        for (int i = 0; i < _materials.Length; i++)
        {
            _materials[i].SetFloat("_FlashAmount", amount);
        }
    }
}


