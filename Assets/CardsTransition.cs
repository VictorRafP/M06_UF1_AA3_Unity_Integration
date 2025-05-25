using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsTransition : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 direction;
    public float speed;
    void Start()
    {
        direction = new Vector3(1, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if (speed > 0)
        {
            transform.position += direction * speed * Time.fixedDeltaTime;
        }
        if (transform.position.x >= 952)
        {
            speed = 0;
        }

    }
}
