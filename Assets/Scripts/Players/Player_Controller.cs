using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Controller : MonoBehaviour
{
    bool IsMoving
    {
        set
        {
            isMoving = value;
            animator.SetBool("isMoving", isMoving);
        }
    }
    public bool showshop;
    public GameObject panel;
    public float moveSpeed = 50f;
    public float maxSpeed = 4f;
    public bool canMove = true;
    public float idleFriction = 0.9f;

    // audio
    public AudioClip swordSwingSound;
    AudioSource audioSword;

    public GameObject swordHitBox;

    Collider2D swordCollider;

    SpriteRenderer spriteRenderer;
    Vector2 movementInput = Vector2.zero;
    Rigidbody2D rb;

    bool isMoving = false;

    Animator animator;

    // Dash variables
    public float dashSpeed = 10f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    private bool isDashing = false;
    private float dashTime = 0f;
    private float dashCooldownTime = 0f;
    private float timeToWait = 0.25f;

    // Particle system for dashing
    public ParticleSystem dashParticleSystem;
    public ParticleSystem slashParticleSystem;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        swordCollider = swordHitBox.GetComponent<Collider2D>();

        audioSword = GetComponent<AudioSource>();

        // Ensure the particle system is initially disabled
        if (dashParticleSystem != null)
        {
            dashParticleSystem.Stop();
        }

        // Ensure the particle system is initially disabled
        if (slashParticleSystem != null)
        {
            slashParticleSystem.Stop();
        }
    }

    private void Update()
    {
        if (showshop && Input.GetKey(KeyCode.E))
        {
            Time.timeScale = 0f;
            panel.SetActive(true);
        }

        if (dashCooldownTime > 0)
        {
            dashCooldownTime -= Time.deltaTime;
        }

        if (isDashing)
        {
            dashTime -= Time.deltaTime;
            if (dashTime <= 0)
            {
                EndDash();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space) && dashCooldownTime <= 0)
        {
            StartDash();
            StartCoroutine(WaitAndPrint());
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("shop"))
        {
            showshop = true;
        }
    }
    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        // if movement input != 0, try to move
        if (canMove == true && movementInput != Vector2.zero)
        {
            // this dont allow player to run faster than the max speed
            //rb.velocity = Vector2.ClampMagnitude(rb.velocity + (movementInput * moveSpeed * Time.deltaTime), maxSpeed);

            rb.AddForce(movementInput * moveSpeed * Time.deltaTime);

            if(rb.velocity.magnitude > maxSpeed)
            {
                float limitedSpeed = Mathf.Lerp(rb.velocity.magnitude, maxSpeed, idleFriction);
                rb.velocity = rb.velocity.normalized * limitedSpeed;
            }

            // control whether looking left or right
            if(movementInput.x > 0)
            {
                spriteRenderer.flipX = false;
                
                // flip the sword
                gameObject.BroadcastMessage("isFacingRight", true);
            } else if(movementInput.x < 0)
            {
                spriteRenderer.flipX = true;
                
                // flip the sword
                gameObject.BroadcastMessage("isFacingRight", false);
            }

            IsMoving = true;
        }
        else {
            // no movement so interpolate velocity toward 0
            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, idleFriction);

            IsMoving = false;
        }

        
        
    }

    // Stop the player alittle bit after dashing
    IEnumerator WaitAndPrint()
    {
        yield return new WaitForSeconds(timeToWait);
        //Debug.Log("Đã chờ đủ " + timeToWait + " giây");
        // Thực hiện hành động ở đây
        canMove = true;
    }

    IEnumerator WaitForSlashPartical()
    {
        yield return new WaitForSeconds(timeToWait);
        // Thực hiện hành động ở đây
        // Stop the particle system
        if (slashParticleSystem != null)
        {
            slashParticleSystem.Stop();
        }
    }

    void PlaySwordSwingSound()
    {
        if (audioSword != null && swordSwingSound != null)
        {
            audioSword.PlayOneShot(swordSwingSound);
            Debug.Log("chay am thanh kiem");
        }
    }

    // get input value for player movement
    void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }

    // slash
    void OnFire()
    {
        animator.SetTrigger("swordAttack");
        // Start the particle system
        if (slashParticleSystem != null)
        {
            slashParticleSystem.Play();
        }
        StartCoroutine(WaitForSlashPartical());
        PlaySwordSwingSound();
    }

    void LockMovement()
    {
        canMove = false;
    }

    void UnLockMovement()
    {
        canMove = true;
    }

    void StartDash()
    {
        isDashing = true;
        dashTime = dashDuration;
        dashCooldownTime = dashCooldown;

        Vector2 dashDirection = movementInput.normalized;
        if (dashDirection == Vector2.zero)
        {
            dashDirection = spriteRenderer.flipX ? Vector2.left : Vector2.right;
        }

        rb.velocity = dashDirection * dashSpeed;

        // Start the particle system
        if (dashParticleSystem != null)
        {
            dashParticleSystem.Play();
        }

        canMove = false;
    }

    void EndDash()
    {
        isDashing = false;

        // Stop the particle system
        if (dashParticleSystem != null)
        {
            dashParticleSystem.Stop();
        }
    }
}
