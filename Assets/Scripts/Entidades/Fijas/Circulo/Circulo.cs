using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circulo : MonoBehaviour
{
    Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Muerte()
    {
       // GameObject temp = Instantiate(particulasMuerte, transform.position, transform.rotation);
        //Destroy(temp, 5);
        //if (GameManager.instance.life > 1)
        //{
        //    StartCoroutine(Respawn_Corutine());
        //}
        //else
        //{
        //    GameManager.instance.life = 0;
        //    Destroy(gameObject);
        //}
    }
}
