using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelClicks : MonoBehaviour
{
    public GameObject panel;
    public Disparos disparos;
    public float maxTime = 10f;
    public float maxCargaTime = 5f;
    public float actualTime = 0f;
    public float actualTime2 = 0f;

    private void Start()
    {
        disparos = FindObjectOfType<Disparos>();
    }

    void Update()
    {
        if (disparos.cadence == 0.25f)
        {
            actualTime2 += Time.deltaTime;
        }
        actualTime += Time.deltaTime;
        if (actualTime >= maxTime)
        {
            Instantiate(panel);
            Destroy(panel, 3f);
            actualTime = 0f;
        }
        if (actualTime2 >= maxCargaTime)
        {
            disparos.cadence = 0.5f;
            actualTime2 = 0f;
        }
    }
}
