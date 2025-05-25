using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanciarRayodef : MonoBehaviour
{
    public GameObject rayo;
    public float maxTime = 1;

    private float currentTime = 0;
    private float posY = -0.63f;
    private Vector2 posRayo;
    void Start()
    {
        
    }

    void Update()
    {
        if (currentTime < maxTime)
        {
            currentTime += Time.deltaTime;
        }
        else
        {
            posRayo = new Vector2(transform.position.x, posY);
            InstanciarRayo(posRayo);
            currentTime = 0;
        }
    }

    void InstanciarRayo(Vector2 possRayo)
    {
        GameObject tira = Instantiate(rayo, possRayo, rayo.transform.rotation);
        Destroy(tira, .5f);
    }
}
