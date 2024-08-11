using UnityEngine;

public class AxeMonster : MonoBehaviour
{
    public float attackDamage = 10f; // Sát thương của đòn chém
    public float attackRange = 2f;   // Khoảng cách tấn công
    public float attackCooldown = 1.5f; // Thời gian hồi chiêu của đòn đánh

    public Transform attackPoint; // Điểm xuất phát của đòn đánh (nơi rìu vung)
    public LayerMask playerLayer; // Layer của nhân vật người chơi

    private float nextAttackTime = 0f; // Thời điểm có thể tấn công tiếp theo
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Kiểm tra khoảng cách giữa quái vật và nhân vật người chơi
        Collider2D playerCollider = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);

        if (playerCollider != null && Time.time >= nextAttackTime)
        {
            Attack(playerCollider.gameObject);
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    private void Attack(GameObject player)
    {
        // Gọi animation tấn công
        animator.SetTrigger("isMinaSlash");

        // Gây sát thương cho nhân vật
        /*IDamagable damageable = player.GetComponent<IDamagable>();
        if (damageable != null)
        {
            damageable.OnHit(attackDamage);
        }*/
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}

