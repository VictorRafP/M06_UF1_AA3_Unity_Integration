using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CorazonesUI : MonoBehaviour
{
    private List<Image> hearts = new List<Image>();
    private Color activeColor = Color.white;
    private Color inactiveColor = Color.grey;

    private void Start()
    {
        foreach (Transform child in transform)
        {
            Image heart = child.GetComponent<Image>();
            if (heart != null) hearts.Add(heart);
        }

        PlayerLifeManager.Instance.OnLifeChanged.AddListener(UpdateHearts);
        UpdateHearts(PlayerLifeManager.Instance.maxLives);
    }

    private void UpdateHearts(int currentLives)
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].color = (i < currentLives) ? activeColor : inactiveColor;
        }
    }
}