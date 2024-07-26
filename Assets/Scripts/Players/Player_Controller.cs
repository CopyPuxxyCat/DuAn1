using System.Collections;
using System.Collections.Generic;
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

   

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        swordCollider = swordHitBox.GetComponent<Collider2D>();
    }

    private void Update()
    {                
    }

    private void FixedUpdate()
    {
        // if movement input != 0, try to move
        if(canMove == true && movementInput != Vector2.zero)
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

    
    // get input value for player movement
    void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }

    // slash
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
    
}
