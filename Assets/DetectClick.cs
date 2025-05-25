using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectClick : MonoBehaviour
{
    public InstantiateClick ClickManager;
    public Disparos Carga;

    private Camera Cam;
    private Vector2 pos;
    void Start()
    {
        Cam = Camera.main;
        ClickManager = FindObjectOfType<InstantiateClick>();
        Carga = FindObjectOfType<Disparos>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(1)) {
            Vector2 pos = Cam.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);

            if (hit.collider != null && hit.transform == transform)
            {
                ClickManager.ActualClicks += 1;
                Carga.cargaActual += 1;
                Destroy(gameObject);
            }
        }
    }
}
