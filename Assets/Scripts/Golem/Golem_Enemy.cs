using UnityEngine;

public class Golem_Enemy : MonoBehaviour
{
    bool IsGolemMoving
    {
        set
        {
            isGolemMoving = value;
            animator.SetBool("isSlimeMoving", isGolemMoving);
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

    public float damage = 1;
    public float knockbackForce = 2f;
    public float moveSpeed = 50f;
    private float bulletSpeed = 1.2f;
    public DetectionZone detectionZone;
    public DetectionZone shootingZone; // Thêm vùng bắn
    public GameObject arrowPrefab; // Prefab của cung tên
    public Transform arrowSpawnPoint; // Điểm xuất phát của cung tên
    public Rigidbody2D rb;
    public float idleFriction = 0.9f;
    public float shootingInterval = 3f; // Khoảng thời gian giữa các lần bắn
    Animator animator;
    bool isGolemMoving = false;
    bool isShooting = false;
    DamageableCharacter damageableCharacter;
    float lastShootTime;

    public float aimTime = 0.5f;  // Thời gian chờ để chạy animation bắn
    private float aimStartTime;
    private bool isAiming = false;

    private bool facingRight = true;

    private int shootCount = 0;

    private void Start()
    {
        rb.GetComponent<Rigidbody2D>();
        damageableCharacter = GetComponent<DamageableCharacter>();
        animator = GetComponent<Animator>();

        lastShootTime = -shootingInterval;  // Đặt thời gian bắn cuối cùng là trừ đi khoảng thời gian bắn để Golem có thể bắn ngay lập tức
    }

    private void FixedUpdate()
    {
        if (damageableCharacter.targetAble && detectionZone.detectedObjs.Count > 0)
        {
            Transform target = detectionZone.detectedObjs[0].transform;
            Vector2 direction = (target.position - transform.position).normalized;
            

            if (shootingZone.detectedObjs.Exists(obj => obj.transform == target))
            {
                // Trong vùng bắn
                rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, idleFriction);
                IsGolemMoving = false;

                if (!isAiming && Time.time > lastShootTime + shootingInterval)
                {
                    isAiming = true;
                    IsShooting = true;
                    aimStartTime = Time.time;
                }
                if (isAiming)
                {
                    if (Time.time > aimStartTime + aimTime)
                    {
                        // Hoàn thành quá trình ngắm bắn và bắn tên
                        KilledEnemy.bulletVector2 = direction;
                        ShootArrow(direction);
                        lastShootTime = Time.time;
                        isAiming = false;
                        IsShooting = false;  // Tắt trạng thái animation bắn
                    }
                }
            }
            else
            {
                // Trong vùng phát hiện nhưng không trong vùng bắn
                rb.AddForce(direction * moveSpeed * Time.deltaTime);
                IsGolemMoving = true;
                isAiming = false;
                IsShooting = false;
                
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
            IsGolemMoving = false;
            IsShooting = false;
        }
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
        if (shootCount < 2)
        {
            // Bắn một viên đạn theo hướng chỉ định
            GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, Quaternion.identity);
            arrow.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
        }
        else
        {
            // Bắn 36 viên đạn theo 360 độ
            for (int i = 0; i < 18; i++)
            {
                float angle = i * 20; // 360 độ chia cho 36 viên đạn
                Vector2 shootDirection = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
                GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, Quaternion.identity);
                arrow.GetComponent<Rigidbody2D>().velocity = shootDirection * bulletSpeed;
            }
            shootCount = -1; // Reset đếm sau khi bắn 36 viên đạn
        }
        shootCount++;
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
