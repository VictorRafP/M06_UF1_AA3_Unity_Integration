using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Collider2D))]
public class CollectibleItem : MonoBehaviour
{
    private BossFaseManager manager;

    public void Initialize(BossFaseManager mgr)
    {
        manager = mgr;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (manager != null && col.CompareTag("player"))
        {
            manager.OnGemCollected();
            gameObject.SetActive(false);
        AudioManager.Instance.PlaySFXFromEnum(AudioManager.SFX_Sounds.GEM);
        }
    }
}
