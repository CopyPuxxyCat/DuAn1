using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
    public bool ShowgiaoTiep;
    public GameObject Panel_giaotiep,Hthoai1,Hthoai2,Hthoai3;
    public GameObject panel;
    public float moveSpeed = 5f;
    public float maxSpeed = 7f;
    public bool canMove = true;
    public float idleFriction = 0.9f;

    // audio
    public AudioClip swordSwingSound;
    AudioSource audioSword;
    public AudioClip healsound;
    AudioSource audioHeal;
    public AudioClip Dash;
    AudioSource audioDash;

    public GameObject swordHitBox;

    Collider2D swordCollider;

    SpriteRenderer spriteRenderer;
    Vector2 movementInput = Vector2.zero;
    Rigidbody2D rb;

    bool isMoving = false;

    Animator animator;

    // Dash variables
    public float dashSpeed = 28f;
    public float dashDuration = 0.0005f;
    public float dashCooldown = 1.5f;
    private bool isDashing = false;
    private float dashTime = 0f;
    private float dashCooldownTime = 0f;

    public Slider thanhmau_Player;

    // Particle system for dashing
    public ParticleSystem dashParticleSystem;

    // Stop movement after dash variables
    public float stopDuration = 0.5f;
    private bool isStopping = false;
    private float stopTime = 0f;
    public float timeToWait = 0.1f;

    // Mage shjt
    private bool isMage = false;
    public Sprite mageSprite;
    public RuntimeAnimatorController mageAnimator;
    public GameObject fireballPrefab;
    public Transform fireballSpawnPoint;
    public float fireballSpeed = 10f;
    private Sprite originalSprite;
    private RuntimeAnimatorController originalAnimator;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        swordCollider = swordHitBox.GetComponent<Collider2D>();

        audioSword = GetComponent<AudioSource>();
        audioHeal = GetComponent<AudioSource>();
        audioDash = GetComponent<AudioSource>();
        // sprite and animator goc cua player
        originalSprite = spriteRenderer.sprite;
        originalAnimator = animator.runtimeAnimatorController;

        // Ensure the particle system is initially disabled
        if (dashParticleSystem != null)
        {
            dashParticleSystem.Stop();
        }
    }

    public GameObject portalPrefab; // Prefab của cổng dịch chuyển
    public float distanceToPlayer = 1f; // Khoảng cách từ người chơi đến cổng

    private bool hasPortal = false;

    public void CreatePortalAtPlayerRight()
    {
        if (!hasPortal)
        {
            // Lấy vị trí của người chơi
            Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;

            // Tính toán vị trí của cổng dịch chuyển
            Vector3 portalPosition = playerPosition + Vector3.right * distanceToPlayer;

            // Tạo cổng dịch chuyển
            Instantiate(portalPrefab, portalPosition, Quaternion.identity);

            hasPortal = true;
        }
    }

    public void PlayerRegen()
    {
        
        if(KilledEnemy.TongSoBinhMau > 0 && KilledEnemy.TongSoBinhMau < 11)
        {
            KilledEnemy.player_health_Manager++;
            KilledEnemy.isHealthRegen = true;
        }
    }

    private void Update()
    {
        Debug.Log("giet mina chua: " + KilledEnemy.isMinotaurKilled);
        thanhmau_Player.value = KilledEnemy.player_health_Manager;

        if(KilledEnemy.isMinotaurKilled == true)
        {
            CreatePortalAtPlayerRight();
        }


        // hoi mau
        if (Input.GetKeyDown(KeyCode.H))
        {
            PlayerRegen();
            PlayHealsound();
        }

            // bien hinh
            if (Input.GetKeyDown(KeyCode.R))
        {
            if (isMage)
            {
                TransformToPlayer();
                animator.SetBool("isAlive", true);

            }
            else
            {
                TransformToMage();
                animator.SetBool("isAlive", true);
            }
        }

        // Update dash cooldown
        if (dashCooldownTime > 0)
        {
            dashCooldownTime -= Time.deltaTime;
        }

        // Handle dashing
        if (isDashing)
        {
            dashTime -= Time.deltaTime;
            if (dashTime <= 0)
            {
                EndDash();
            }
        }
        else if (isStopping)
        {
            stopTime -= Time.deltaTime;
            if (stopTime <= 0)
            {
                isStopping = false;
                canMove = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space) && dashCooldownTime <= 0)
        {
            StartDash();
            //StartCoroutine(WaitAfterDashToMove());
            PlayDashSound();
        }

        if (showshop && Input.GetKey(KeyCode.E))
        {
            Time.timeScale = 0f;
            panel.SetActive(true);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            panel.SetActive(false);
            Time.timeScale = 1f;
        }
        if(ShowgiaoTiep && Input.GetKey(KeyCode.E))
        {
            Time.timeScale = 1f;
            Panel_giaotiep.SetActive(true);
            Hthoai2.SetActive(false);
            Hthoai3.SetActive(false);
        }
    }
    

    // shop
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("shop"))
        {
            showshop = true;
        }
        if (collision.gameObject.CompareTag("NPC"))
        {
            ShowgiaoTiep = true;
        }
    }
 
    private void FixedUpdate()
    {
        
        // Prevent movement during dash and stop period
        if (isDashing || isStopping)
        {
            return;
        }


        // if movement input != 0, try to move
        if (canMove == true && movementInput != Vector2.zero)
        {
            // this dont allow player to run faster than the max speed
            //rb.velocity = Vector2.ClampMagnitude(rb.velocity + (movementInput * moveSpeed * Time.deltaTime), maxSpeed);

            //rb.AddForce(movementInput * moveSpeed * Time.deltaTime);
            transform.Translate(movementInput * moveSpeed * Time.deltaTime);

            if (rb.velocity.magnitude > maxSpeed)
            {
                float limitedSpeed = Mathf.Lerp(rb.velocity.magnitude, maxSpeed, idleFriction);
                rb.velocity = rb.velocity.normalized * limitedSpeed;
            }
            // control whether looking left or right
            if (movementInput.x > 0)
            {
                spriteRenderer.flipX = false;

                // flip the sword
                gameObject.BroadcastMessage("isFacingRight", true);
            }
            else if (movementInput.x < 0)
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

    // bien hinh
    void TransformToMage()
    {
        if (isMage) return;
        isMage = true;

        // Reset trigger hoặc các điều kiện liên quan đến trạng thái die nếu cần thiết
        //animator.ResetTrigger("die");
        animator.SetBool("isAlive", true);

        spriteRenderer.sprite = mageSprite;
        animator.runtimeAnimatorController = mageAnimator;
        Debug.Log("Biến hình thành Mage!");
    }

    void TransformToPlayer()
    {
        if (!isMage) return;
        isMage = false;

        //animator.ResetTrigger("die");
        animator.SetBool("isAlive", true);

        spriteRenderer.sprite = originalSprite; // Biến originalSprite cần được lưu trữ khi Start.
        animator.runtimeAnimatorController = originalAnimator; // Biến originalAnimator cần được lưu trữ khi Start.
        Debug.Log("Biến trở lại thành Player!");
    }

    IEnumerator WaitAfterDashToMove()
    {
        yield return new WaitForSeconds(timeToWait);
        // Thực hiện hành động ở đây
        canMove = true;
    }

    IEnumerator WaitAfterDashToMove_2()
    {
        yield return new WaitForSeconds(timeToWait);
        // Thực hiện hành động ở đây
        rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, idleFriction);
    }

    void PlaySwordSwingSound()
    {
        if (audioSword != null && swordSwingSound != null)
        {
            audioSword.PlayOneShot(swordSwingSound);
        }
    }
    void PlayHealsound()
    {
        if (audioHeal != null && healsound != null)
        {
            audioHeal.PlayOneShot(healsound);
        }
    }
    void PlayDashSound()
    {
        if (audioDash != null && Dash != null)
        {
            audioDash.PlayOneShot(Dash);
        }
    }

    // get input value for player movement
    void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }

    // slash and shoot
    void OnFire()
    {
        if (isMage)
        {
            FireFireball();
            animator.SetTrigger("swordAttack");
            PlaySwordSwingSound();
        }
        else
        {
            animator.SetTrigger("swordAttack");
            PlaySwordSwingSound();
        }
    }

    void FireFireball()
    {
        if (canMove == true)
        {
            Vector2 fireDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - fireballSpawnPoint.position;
            fireDirection.Normalize();

            GameObject fireball = Instantiate(fireballPrefab, fireballSpawnPoint.position, Quaternion.identity);
            fireball.GetComponent<Rigidbody2D>().velocity = fireDirection * fireballSpeed;

            float angle = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg;
            fireball.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }
    /*void OnFire()
    {
        animator.SetTrigger("swordAttack");
        PlaySwordSwingSound();
    }*/

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
        //canMove = false;

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
    }

    void EndDash()
    {
        isDashing = false;
        isStopping = true;
        stopTime = stopDuration;
        Debug.Log("goi endDash");

        StartCoroutine(WaitAfterDashToMove_2());
        // Stop the particle system
        if (dashParticleSystem != null)
        {
            dashParticleSystem.Stop();
        }
        canMove = false;
    }
}
