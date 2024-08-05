using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem_Enemy : MonoBehaviour
{
    bool IsGolemMoving
    {
        set
        {
            isGolemMoving = value;
            animator.SetBool("isGolemMoving", isGolemMoving);
        }
    }

    public float damage = 1;
    public float knockbackForce = 2f;
    public float moveSpeed = 50f;
    public DetectionZone detectionZone;
    public DetectionZone shootingZone; // Thêm vùng bắn
    public Rigidbody2D rb;
    public float idleFriction = 0.9f;
    public GameObject arrowPrefab; // Thêm đối tượng cung tên
    public float shootCooldown = 2f; // Thời gian hồi giữa các lần bắn

    Animator animator;
    bool isGolemMoving = false;
    DamageableCharacter damageableCharacter;
    float lastShootTime;

    private void Start()
    {
        rb.GetComponent<Rigidbody2D>();
        damageableCharacter = GetComponent<DamageableCharacter>();
        animator = GetComponent<Animator>();
        lastShootTime = -shootCooldown; // Đảm bảo Golem có thể bắn ngay lập tức
    }

    private void FixedUpdate()
    {
        if (/*damageableCharacter.targetAble &&*/ detectionZone.detectedObjs.Count > 0)
        {
            // calculate direction to target
            Vector2 direction = (detectionZone.detectedObjs[0].transform.position - transform.position).normalized;
            Debug.Log("thay nhan vat");
            // move toward the objs
            rb.AddForce(direction * moveSpeed * Time.deltaTime);
            IsGolemMoving = true;
        }
        else
        {
            // no movement so interpolate velocity toward 0
            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, idleFriction);
            IsGolemMoving = false;
        }

        if (shootingZone.detectedObjs.Count > 0 && Time.time >= lastShootTime + shootCooldown)
        {
            ShootArrow(shootingZone.detectedObjs[0].transform);
            lastShootTime = Time.time;
        }
    }

    void ShootArrow(Transform target)
    {
        Vector2 direction = (target.position - transform.position).normalized;
        GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
        arrow.GetComponent<Rigidbody2D>().velocity = direction * moveSpeed;
        // Đặt thêm các thuộc tính khác của mũi tên nếu cần
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

