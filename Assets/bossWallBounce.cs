using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossWallBounce : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private BoxCollider2D bc2d;
    public int bounceForce;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("player"))
        {
            
            rb2d = collision.gameObject.GetComponent<Rigidbody2D>();
            rb2d.velocity = Vector3.zero;
            collision.transform.position += new Vector3(0,.1f, 0);
            rb2d.AddForce(new Vector3(1, .5f, 0) * bounceForce);
            


        }
    }
}
