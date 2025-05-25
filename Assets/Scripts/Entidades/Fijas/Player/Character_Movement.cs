using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

public enum Direction { NONE, UP, DOWN, LEFT, RIGHT };
public class Character_Movement : MonoBehaviour
{
    public float speedX = 6;
    //public float speedY = 16;
    float horizontal = 0;
    float vertical = 0;
    float jump;
    public bool canJump;
    float canJumpF;
    public float jumpForce = 250f;

    public bool dashing;
    public float currentTimeDash = 0;
    public float maxTimeDash = 0.2f;
    public float dashMaxCooldown = 2.5f;
    public float dashCooldown = 0;
    public float alturamin = -1.8f;
    private bool isGrounded = false;

    public Animator animator;
    Rigidbody2D rb2d;

    public Direction direction = Direction.NONE;
    [Header("RaycastInfo")]
    public float RaycastFloorDistance = 0.15f;
    public float RaycastFloorOriginOffset = -0.85f;
    public float RaycastTopDistance = 0f;
    public float RaycastTopOriginOffset = 0f;
    public float RaycastTopGap = 0.15f;
    public float RaycastFloorGap = 0.15f;
    public bool TopRayL = false;
    public bool TopRayR = false;
    public bool TopRayCenter = false;

    //public GameObject Personaje;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        jump = Input.GetAxis("Jump");

        if (!canJump)
        {
            canJumpF = 1;
        }
        else
        {
            canJumpF = 0;
        }

        float altura = transform.position.y - alturamin;

        animator.SetFloat("mov",horizontal * speedX);
        animator.SetFloat("salto", canJumpF);

        UpdateDirection();
        Dashing();

    }

    private void FixedUpdate()
    {
        //Debug.Log(TopRayL);
        //Debug.Log(TopRayR);
        //Debug.Log(rb2d.velocity.y);
        //Comprueba si los componentes existen antes de usarlos
        if (rb2d == null) { return; } //Si no existe, no hace nada

        //Separamos l�gica de f�sica

        Vector3 direccion = new Vector3(horizontal, 0, 0);
        if(rb2d.velocity.x < 0.1f)
        {
        transform.position += direccion * speedX * Time.fixedDeltaTime;
        }
        //Debug.Log(Input.GetAxis("Vertical"));
        //rb2d.velocity = new Vector2(horizontal * speedX * Time.fixedDeltaTime, rb2d.velocity.y);
        if (rb2d.velocity.y != 0f) { 
                DisplacePlayerJump();
        }

        if (rb2d.velocity.y == 0f)
        {
            if (canJump && jump > 0 && isGrounded)
            {
                rb2d.AddForce(transform.up * jumpForce * rb2d.gravityScale);
            }
        }

        if(canJump && isGrounded)
        {
            rb2d.velocity *= new Vector2(0,1);
        }

        CheckFloor();
        CheckTop();


        //while(transform.position.y > 2.4)
        //{
        //    rb2d.velocity = new Vector2(horizontal * speedX * Time.fixedDeltaTime, rb2d.velocity.y);
        //}



    }

    private void DisplacePlayerJump()
    {
        if (TopRayL && !TopRayR && !TopRayCenter && horizontal >= 0)
        {
            this.gameObject.transform.position = new Vector3(transform.position.x + 0.3f, transform.position.y, transform.position.z);
            TopRayL = false;
        }
        else if (TopRayR && !TopRayL && !TopRayCenter && horizontal <=0)
        {
            this.gameObject.transform.position = new Vector3(transform.position.x - 0.3f, transform.position.y, transform.position.z);
            TopRayR = false;
        }
    }

    private void CheckFloor()
    {
        Vector2 RaycastOriginL = new Vector2(transform.position.x - RaycastFloorGap,transform.position.y + RaycastFloorOriginOffset);
        Vector2 RaycastOriginR = new Vector2(transform.position.x + RaycastFloorGap,transform.position.y + RaycastFloorOriginOffset);
        Vector2 RaycastDir = Vector2.down;
        Debug.DrawRay(RaycastOriginL, RaycastDir * RaycastFloorDistance, UnityEngine.Color.red);
        Debug.DrawRay(RaycastOriginR, RaycastDir * RaycastFloorDistance, UnityEngine.Color.red);
        RaycastHit2D hitL = Physics2D.Raycast(RaycastOriginL, RaycastDir, RaycastFloorDistance);
        RaycastHit2D hitR = Physics2D.Raycast(RaycastOriginR, RaycastDir, RaycastFloorDistance);
        //Debug.DrawLine(RaycastOrigin, hit.point, UnityEngine.Color.green);
        if(hitL.collider != null)
        {
            if (hitL.collider.CompareTag("Suelo"))
            {
                //Debug.Log("Raycast hit: " + hit.collider.name)
                //Debug.Log("TouchingGround");
                isGrounded = true;

            }
        }

        if (hitR.collider != null)
        {
            if (hitR.collider.CompareTag("Suelo"))
            {
                //Debug.Log("Raycast hit: " + hit.collider.name)
                //Debug.Log("TouchingGround");
                isGrounded = true;
            }
        }
        
        if(hitR.collider == null && hitL.collider == null)
        {
            isGrounded = false;
        }

    }

    private void CheckTop()
    {
        Vector2 RaycastOriginL = new Vector2(transform.position.x - RaycastTopGap, transform.position.y + RaycastTopOriginOffset);
        Vector2 RaycastOriginR = new Vector2(transform.position.x + RaycastTopGap, transform.position.y + RaycastTopOriginOffset);
        Vector2 RaycastOriginCenter = new Vector2(transform.position.x, transform.position.y + RaycastTopOriginOffset);
        Vector2 RaycastDir = Vector2.up;
        Debug.DrawRay(RaycastOriginL, RaycastDir * RaycastTopDistance, UnityEngine.Color.cyan);
        Debug.DrawRay(RaycastOriginR, RaycastDir * RaycastTopDistance, UnityEngine.Color.magenta);
        Debug.DrawRay(RaycastOriginCenter, RaycastDir * RaycastTopDistance, UnityEngine.Color.red);
        RaycastHit2D hitL = Physics2D.Raycast(RaycastOriginL, RaycastDir, RaycastTopDistance);
        RaycastHit2D hitR = Physics2D.Raycast(RaycastOriginR, RaycastDir, RaycastTopDistance);
        RaycastHit2D hitCenter = Physics2D.Raycast(RaycastOriginCenter, RaycastDir, RaycastTopDistance);

        if (hitL.collider != null)
        {
            if(hitL.collider.CompareTag("Wall"))
            {
                TopRayL = true;
                Debug.Log("HitLeft");
            }

        }
        else
        {
            TopRayL = false;
        }

        if(hitR.collider != null)
        {
            if (hitR.collider.CompareTag("Wall"))
            {
                TopRayR = true;
                Debug.Log("HitRight");
            }
        }
        else
        {
            TopRayR = false;
        }

        if(hitCenter.collider != null)
        {
            if (hitCenter.collider.CompareTag("Wall"))
            {
                TopRayCenter = true;
                Debug.Log("HitCenter");
            }
        }
        else
        {
            TopRayCenter = false;
        }
    }
    private void UpdateDirection()
    {
        if (dashing) { return; }
        direction = Direction.NONE;
        int horizontal = 0;
        int vertical = 0;

        if (Input.GetKey(KeyCode.A)) { horizontal -= 1; }
        if (Input.GetKey(KeyCode.D)) { horizontal += 1; }


        if (vertical > 0)
        {
            direction = Direction.UP;
        }
        else if (vertical < 0)
        {
            direction = Direction.DOWN;
        }

        if (horizontal < 0)
        {
            direction = Direction.LEFT;
        }
        else if (horizontal > 0)
        {
            direction = Direction.RIGHT;
        }
    }

    private void Dashing()
    {
        CooldownDash();
        if (dashing)
        {
            currentTimeDash += Time.deltaTime;
            if (currentTimeDash >= maxTimeDash)
            {
                endDashing();
            }
        }
        if (dashCooldown <= 0)
        {
            if (Input.GetKey(KeyCode.LeftShift) && direction != Direction.NONE)
            {
                Dash();
                AudioManager.Instance.PlaySFXFromEnum(AudioManager.SFX_Sounds.DASH);
            }
        }
    }
    private void Dash()
    {
        speedX = 20;
        currentTimeDash = 0;
        dashCooldown = dashMaxCooldown;
        dashing = true;
    }
    private void endDashing()
    {
        speedX = 6;
        currentTimeDash = 0;
        dashing = false;
    }
    private void CooldownDash()
    {
        if (dashCooldown > 0)
        {
            dashCooldown -= Time.deltaTime;
            if (dashCooldown <= 0)
            {
                dashCooldown = 0;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            canJump = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            canJump = false;
        }
    }

    //public void speedToZero()
    //{
    //    rb2d.velocity = new Vector3(0, 0, 0);
    //}

}
