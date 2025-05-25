using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowShield : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject shieldObject;
    private SpriteRenderer shieldRenderer;
    void Start()
    {
        shieldRenderer = shieldObject.GetComponent<SpriteRenderer>();
        shieldRenderer.color *= new Color(1,1,1,0);
        shieldObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
