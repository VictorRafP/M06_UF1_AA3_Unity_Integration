using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMover : MonoBehaviour
{

    Vector3 movimiento = new Vector3(1, 0, 0);
    public int ID = 0;
    public float speed = 1;
    public Animator animator;
    public float CurrentTime = 0f;
    public float fixedCooldown = 0.5f;
    public SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        switch (ID)
        {
            case 0:
                speed = speed * 3;
                if (transform.position.x < 0)
                {
                    spriteRenderer.flipX = true;
                }
                transform.position = new Vector3(transform.position.x, -2.18f, 0);
                break;
            case 1:
                if (transform.position.x > 0)
                {
                    spriteRenderer.flipX = true;
                }
                transform.position = new Vector3(transform.position.x, -2.45f, 0);
                break;
            case 2:
                speed = speed * (float)2.5;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ID == 0)
        {
            if (transform.position.x > 2)
            {
                transform.position += movimiento * -speed * Time.deltaTime;
            }
            else if (transform.position.x < -2)
            {
                transform.position += movimiento * speed * Time.deltaTime;
            }
            else
            {
                if (CurrentTime < fixedCooldown)
                {
                    CurrentTime += Time.deltaTime;
                }
                else
                {
                    Destroy(gameObject);
                    CurrentTime = 0f;
                }
            }

            if (transform.position.x < 2.5 && transform.position.x > -2.5)
            {
                animator.SetFloat("saltos", 3);
            }
            else if ((transform.position.x >= 2.5 && transform.position.x <= 4.5) || (transform.position.x <= -2.5 && transform.position.x >= -4.5))
            {
                animator.SetFloat("saltos", 2);
            }
            else if ((transform.position.x > 4.5 && transform.position.x <= 6.5) || (transform.position.x < -4.5 && transform.position.x >= -6.5))
            {
                animator.SetFloat("saltos", 1);
            }
            else
            {
                animator.SetFloat("saltos", 0);
            }
        }
        else if (ID == 1)
        {
            if (transform.position.x > 2)
            {
                transform.position += movimiento * -speed * Time.deltaTime;
            }
            else if (transform.position.x < -2)
            {
                transform.position += movimiento * speed * Time.deltaTime;
            } 
        }
        else if (ID == 2)
        {
            transform.position += movimiento * speed * Time.deltaTime;
            if (transform.position.x > 13 || transform.position.x < -13)
            {
                speed = -speed;
            }
            animator.SetFloat("direccion", transform.position.x * -1);
        }
    }
}
