using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhongLonBay : MonoBehaviour
{
    // Start is called before the first frame update
    public float bulletDamage = 1f;
    public float knockbackForce = 2f;
    public Collider2D bulletCollider2;
    public float timeToWait = 0.5f;
    Animator animator;

    public float returnTime = 4f; // Thời gian trước khi mũi tên quay lại
    private Transform target;
    private bool returning = false;
    private Rigidbody2D rb;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (bulletCollider2 == null)
        {
            Debug.LogWarning("need to set bullet collider!");
        }

        rb = GetComponent<Rigidbody2D>();
        //Destroy(gameObject, 10f); // Tự hủy sau 5 giây nếu không quay về

        StartCoroutine(WaitAfterShoot());
    }

    public void ReturnToSender(Transform sender)
    {
        target = sender;
        Invoke("StartReturning", returnTime); // Bắt đầu quay lại sau một thời gian nhất định
    }

    private void StartReturning()
    {
        returning = true;
        
    }

    public float rotationSpeed = 1500f; // Tốc độ xoay của viên đạn (độ trên giây)

    private void FixedUpdate()
    {
        if (returning && target != null)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            rb.velocity = direction * KilledEnemy.phongLonSpeed;

            // Xoay hướng của mũi tên khi quay về
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        }
    }

    void Update()
    {
        // Xoay viên đạn xung quanh trục Z
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

        Debug.Log("dan duoc ban ra");
    }

    

    IEnumerator WaitAfterShoot()
    {
        //animator.SetTrigger("bulletHit");
        yield return new WaitForSeconds(timeToWait);
        // Thực hiện hành động ở đây
        bulletCollider2.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (returning == true && collision.CompareTag("MeleZone"))
        {
            Destroy(gameObject);  // Hủy mũi tên nếu nó chạm vào MeleZone
        }
        else if (returning == false)
        {
            IDamagable damagealeObject = collision.GetComponent<IDamagable>();

            // calculate direction between character and slime
            if (damagealeObject != null)
            {

                // knockback is in direction of bulletCollider towards collider
                Vector2 knockback = KilledEnemy.bulletVector2 * knockbackForce;

                // make it hit by passing the Vector2 force to the rb
                damagealeObject.OnHit(bulletDamage, knockback);

                Destroy(gameObject);

                Debug.Log("dinh dmg");
            }
        }
    }
}
