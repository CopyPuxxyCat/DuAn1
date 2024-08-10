using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Minotaur : MonoBehaviour
{
    bool IsMinotaurMoving
    {
        set
        {
            isMinotaurMoving = value;
            animator.SetBool("isMinoMoving", isMinotaurMoving);
        }
    }

    bool IsShooting
    {
        set
        {
            isShooting = value;
            animator.SetBool("isShooting", isShooting);
        }
    }

    bool IsSpin
    {
        set
        {
            isSpin = value;
            animator.SetBool("isSpin", isSpin);
        }
    }

    bool IsReadySpin
    {
        set
        {
            isReadySpin = value;
            animator.SetBool("isReadySpin", isReadySpin);
        }
    }

    public float damage = 1;
    public float knockbackForce = 2f;
    public float moveSpeed = 60f;
    private float bulletSpeed = 1.2f;
    public DetectionZone detectionZone;
    public DetectionZone shootingZone; // Thêm vùng bắn
    public GameObject arrowPrefab; // Prefab của cung tên
    public Transform arrowSpawnPoint; // Điểm xuất phát của cung tên
    public Rigidbody2D rb;
    public float idleFriction = 0.9f;
    public float shootingInterval = 3f; // Khoảng thời gian giữa các lần bắn
    Animator animator;
    bool isMinotaurMoving = false;
    bool isShooting = false;
    bool isSpin = false;
    bool isReadySpin = false;
    DamageableCharacter damageableCharacter;
    float lastShootTime;

    bool canMove = true;

    public float aimTime = 0.5f;  // Thời gian chờ để chạy animation bắn
    private float aimStartTime;
    private bool isAiming = false;

    private bool facingRight = true;

    private int comboCount = 0;


    public float attackRate = 1f;

    private float nextAttackTime = 0f;

    private void Start()
    {
        rb.GetComponent<Rigidbody2D>();
        damageableCharacter = GetComponent<DamageableCharacter>();
        animator = GetComponent<Animator>();

        lastShootTime = -shootingInterval;  // Đặt thời gian bắn cuối cùng là trừ đi khoảng thời gian bắn để Minotaur có thể bắn ngay lập tức
    }

    private void FixedUpdate()
    {
        /*if (damageableCharacter.targetAble && detectionZone.detectedObjs.Count > 0)
        {
            Transform target = detectionZone.detectedObjs[0].transform;
            Vector2 direction = (target.position - transform.position).normalized;

            if (shootingZone.detectedObjs.Exists(obj => obj.transform == target))
            {
                    KilledEnemy.bulletVector2 = direction;
                    MeleAttack(direction);
            }
            else
            {
                // Trong vùng phát hiện nhưng không trong vùng bắn
                if (canMove == true)
                {
                    rb.AddForce(direction * moveSpeed * Time.deltaTime);
                    IsMinotaurMoving = true;
                    IsShooting = false;
                }

            }

            // Lật sprite nếu cần
            if ((facingRight && direction.x < 0) || (!facingRight && direction.x > 0))
            {
                Flip();
            }
        }
        else
        {
            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, idleFriction);
            IsMinotaurMoving = false;
            IsShooting = false;
        }*/
    }
    private void Update()
    {
        if (damageableCharacter.targetAble && detectionZone.detectedObjs.Count > 0)
        {
            Transform target = detectionZone.detectedObjs[0].transform;
            Vector2 direction = (target.position - transform.position).normalized;

            if (shootingZone.detectedObjs.Exists(obj => obj.transform == target))
            {
                KilledEnemy.bulletVector2 = direction;
                MeleAttack(direction);
            }
            else
            {
                // Trong vùng phát hiện nhưng không trong vùng bắn
                if (canMove == true)
                {
                    rb.AddForce(direction * moveSpeed * Time.deltaTime);
                    IsMinotaurMoving = true;
                    IsShooting = false;
                }

            }

            // Lật sprite nếu cần
            if ((facingRight && direction.x < 0) || (!facingRight && direction.x > 0))
            {
                Flip();
            }
        }
        else
        {
            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, idleFriction);
            IsMinotaurMoving = false;
            IsShooting = false;
        }
        Debug.Log("cbc" + comboCount);
    }
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    void ShootArrow(Vector2 direction)
    {
            // Bắn một viên đạn theo hướng chỉ định
            GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, Quaternion.identity);
            arrow.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;      
    }

    public float timeToWaitSpin = 0f;

    void MeleAttack(Vector2 direction)
    {
        if (comboCount == 0)
        {          
                animator.SetTrigger("isMinaSlash");
            comboCount = 1;
        }
        else if (comboCount ==1 )
        {
            rb.AddForce(direction * 10f * Time.deltaTime);
            animator.SetTrigger("Dam");
            //comboCount = -1;
            comboCount = 2;
        }
        else if (comboCount == 2)
        {
            // đoạn code để chạy animation isReadySpin trong 4f và sau đó chạy đoạn animation isSpin trong 5f
            StartCoroutine(SpinAttackSequence(direction));
            comboCount = 3;
        }
        else if(comboCount == 3)
        {
            // Bắn một viên đạn theo hướng chỉ định
            GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, Quaternion.identity);
            arrow.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
            comboCount = 0;
        }
        //comboCount++;
    }
    public float spinningSpeed = 50f; 
    private IEnumerator SpinAttackSequence(Vector2 direction)
    {
        // Chạy animation isReadySpin trong 4 giây
        IsReadySpin = true;
        yield return new WaitForSeconds(4f);
        IsReadySpin = false;

        // Chạy animation isSpin trong 5 giây
        //transform.Translate(direction * spinningSpeed * Time.deltaTime);
        IsSpin = true;
        transform.Translate(direction * spinningSpeed * Time.deltaTime);
        yield return new WaitForSeconds(5f);
        IsSpin = false;
        yield return new WaitForSeconds(1f);
    }

    void LockMovement()
    {
        canMove = false;
    }

    void UnLockMovement()
    {
        canMove = true;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Collider2D collider = col.collider;
        IDamagable damageable = collider.GetComponent<IDamagable>();

        if (damageable != null)
        {
            Vector2 direction = (collider.transform.position - transform.position).normalized;
            Vector2 knockback = direction * knockbackForce;
            damageable.OnHit(damage, knockback);
        }
    }
}
