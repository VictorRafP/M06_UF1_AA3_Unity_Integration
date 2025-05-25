using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateClick : MonoBehaviour
{
    public GameObject click;
    public short ClicksNum = 5;
    public short ActualClicks = 0;
    public float Xmin = -8f;
    public float Ymin = -3f;
    public float Xmax = 8f;
    public float Ymax = 3f;
    
    private float posicionX;
    private float posicionY;

    void Start()
    {
        for (short i = 0; i < ClicksNum; i++) {
            posicionX = Random.Range(Xmin, Xmax);
            posicionY = Random.Range(Ymin, Ymax);
            Vector2 pos = new Vector2(posicionX, posicionY);
            Instantiate(click, pos, Quaternion.identity);
        }
    }

    void Update()
    {
        if (ActualClicks >= ClicksNum)
        {
            ActualClicks = 0;
            Destroy(gameObject);
        }
    }
}
