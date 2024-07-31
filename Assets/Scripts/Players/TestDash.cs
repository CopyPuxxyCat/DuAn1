using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestDash : MonoBehaviour
{
    // Các biến hiện có
    bool IsMoving
    {
        set
        {
            isMoving = value;
            animator.SetBool("isMoving", isMoving);
        }
    }

    public float moveSpeed = 50f;
    public float maxSpeed = 4f;
    public bool canMove = true;
    public float idleFriction = 0.9f;

    public GameObject swordHitBox;

    Collider2D swordCollider;

    SpriteRenderer spriteRenderer;
    Vector2 movementInput = Vector2.zero;
    Rigidbody2D rb;

    bool isMoving = false;
    Animator animator;

    // Các biến dash mới
    public float dashSpeed = 10f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    private bool isDashing = false;
    private float dashTime = 0f;
    private float dashCooldownTime = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        swordCollider = swordHitBox.GetComponent<Collider2D>();
    }

    private void Update()
    {
        // Kiểm tra thời gian cooldown của dash
        if (dashCooldownTime > 0)
        {
            dashCooldownTime -= Time.deltaTime;
        }

        // Kiểm tra nếu đang trong dash
        if (isDashing)
        {
            dashTime -= Time.deltaTime;
            if (dashTime <= 0)
            {
                isDashing = false;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space) && dashCooldownTime <= 0)
        {
            StartDash();
        }
    }

    private void FixedUpdate()
    {
        // Không di chuyển khi đang dash
        if (isDashing)
        {
            return;
        }

        if (canMove && movementInput != Vector2.zero)
        {
            rb.AddForce(movementInput * moveSpeed * Time.deltaTime);

            if (rb.velocity.magnitude > maxSpeed)
            {
                float limitedSpeed = Mathf.Lerp(rb.velocity.magnitude, maxSpeed, idleFriction);
                rb.velocity = rb.velocity.normalized * limitedSpeed;
            }

            if (movementInput.x > 0)
            {
                spriteRenderer.flipX = false;
                gameObject.BroadcastMessage("isFacingRight", true);
            }
            else if (movementInput.x < 0)
            {
                spriteRenderer.flipX = true;
                gameObject.BroadcastMessage("isFacingRight", false);
            }

            IsMoving = true;
        }
        else
        {
            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, idleFriction);
            IsMoving = false;
        }
    }

    void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }

    void OnFire()
    {
        animator.SetTrigger("swordAttack");
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
        //rb.velocity = Vector2.Lerp(dashDirection * dashSpeed, Vector2.zero, idleFriction);
        rb.velocity = dashDirection * dashSpeed;
    }
}