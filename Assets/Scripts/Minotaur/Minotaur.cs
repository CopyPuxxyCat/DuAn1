using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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
    public float bulletSpeed = 5f;
    public DetectionZone detectionZone;
    public DetectionZone shootingZone; // Thêm vùng bắn
    public DetectionZone meleZone;
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
    //float lastShootTime = 0;

    public Transform player;

    bool canMove = true;

    public float aimTime = 0.5f;  // Thời gian chờ để chạy animation bắn
    //private float aimStartTime;
    //private bool isAiming = false;

    private bool facingRight = true;

    private int currentComboStep = 0;
    private bool isComboActive = false;


    public float attackRate = 1f;

    private float nextAttackTime = 3f;

    public float spinningSpeed = 1550f;

    // mau
    public Slider thanhmau_Boss;

    private void Start()
    {
        rb.GetComponent<Rigidbody2D>();
        damageableCharacter = GetComponent<DamageableCharacter>();
        animator = GetComponent<Animator>();

        // Tìm đối tượng player (player có tag là "Player")
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //lastShootTime = -shootingInterval;  // Đặt thời gian bắn cuối cùng là trừ đi khoảng thời gian bắn để Minotaur có thể bắn ngay lập tức
    }

    private void FixedUpdate()
    {
        if (isShooting == true)
        {
            return; // Dừng combo nếu quái vật đang bắn mũi tên
        }
        if (damageableCharacter.targetAble && detectionZone.detectedObjs.Count > 0)
        {
            Transform target = detectionZone.detectedObjs[0].transform;
            Vector2 direction = (target.position - transform.position).normalized;

            if (meleZone.detectedObjs.Exists(obj => obj.transform == target))
            {
                MeleAttack(direction);
            }
            /*else if (shootingZone.detectedObjs.Exists(obj => obj.transform == target))
            {
                // Khi ở trong vùng bắn
                //StartCoroutine(FreeShotSequence(direction));
            }*/
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
    }
    private void Update()
    {
        thanhmau_Boss.value = KilledEnemy.boss_health_Manager;
        Debug.Log("thanh mau: " + thanhmau_Boss.value);
        KilledEnemy.phongLonSpeed = bulletSpeed;
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

        if (isShooting) return; // Ngăn chặn việc bắn nếu đã bắn trước đó
        IsShooting = true;
        /*// Bắn một viên đạn theo hướng chỉ định
        GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, Quaternion.identity);
        arrow.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;

    // Xoay hướng của mũi tên
    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    arrow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));*/
        for (int i = 0; i < 6; i++)
        {
            float angle = i * 60; // 360 độ chia cho 36 viên đạn
            Vector2 shootDirection = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, Quaternion.identity);
            arrow.GetComponent<Rigidbody2D>().velocity = shootDirection * bulletSpeed;
            arrow.GetComponent<PhongLonBay>().ReturnToSender(transform);
        }
        // Gọi hàm để viên đạn quay trở lại sau một khoảng thời gian
        //arrow.GetComponent<PhongLonBay>().ReturnToSender(transform);
        // Đặt lại trạng thái isShooting sau một khoảng thời gian
        StartCoroutine(ResetShootingStatus(5f));  // 1 giây là ví dụ, bạn có thể điều chỉnh tùy ý
    }

    private IEnumerator ResetShootingStatus(float delay)
    {
        yield return new WaitForSeconds(delay);
        isShooting = false;
    }

    void MeleAttack(Vector2 direction)
    {
        if (isShooting == true)
        {
            return; // Dừng combo nếu quái vật đang bắn mũi tên
        }

        if (!isComboActive && meleZone.detectedObjs.Count > 0)
        {
            isComboActive = true;
            StartCoroutine(ExecuteCombo(direction));
        }
    }

    private IEnumerator ExecuteCombo(Vector2 direction)
    {
        while (meleZone.detectedObjs.Count > 0 && currentComboStep < 5) // Kiểm tra vùng cận chiến và bước combo
        {
            //shootingZone.GetComponent<Collider2D>().enabled = false;
            // Bước 1: isMinaSlash lần 1
            animator.SetTrigger("isMinaSlash");
            yield return new WaitForSeconds(nextAttackTime); // Thời gian chờ giữa các bước
            currentComboStep++;

            // Bước 1: isMinaSlash lần 1
            animator.SetTrigger("isMinaSlash");
            yield return new WaitForSeconds(nextAttackTime); // Thời gian chờ giữa các bước
            currentComboStep++;

            // Bước 2: isMinaSlash lần 2
            animator.SetTrigger("Dam");
            AddDashForce();
            yield return new WaitForSeconds(nextAttackTime);
            currentComboStep++;

            // Bước 3: Dam lần 1
            animator.SetTrigger("isMinaSlash");
            
            yield return new WaitForSeconds(nextAttackTime);
            currentComboStep++;

            // Bước 4: Dam lần 2
            animator.SetTrigger("Dam");
            AddDashForce();
            yield return new WaitForSeconds(nextAttackTime);
            currentComboStep++;

            // Bước 5: SpinAttackSequence
            
            yield return StartCoroutine(SpinAttackSequence(direction));          
            yield return new WaitForSeconds(nextAttackTime);
            currentComboStep++;

            // Bước 6: Bắn trong 10 giây
            ShootArrow(direction);
            //yield return StartCoroutine(FreeShotSequence(direction));
        }
        // Reset combo sau khi hoàn thành
        isComboActive = false;
        currentComboStep = 0;
    }

    private void AddDashForce()
    {
        if (player == null) return;

        Vector2 direction = (player.position - transform.position).normalized;
        float dashForce = 10f; // Lực đẩy, bạn có thể điều chỉnh tùy ý
        rb.AddForce(direction * dashForce, ForceMode2D.Impulse);
    }

    
    private IEnumerator SpinAttackSequence(Vector2 direction)
    {
        // Chạy animation isReadySpin trong 4 giây
        IsShooting = false;
        IsReadySpin = true;
        yield return new WaitForSeconds(4f);
        IsReadySpin = false;
        AddDashForce();
        // Chạy animation isSpin trong 5 giây
        //transform.Translate(direction * spinningSpeed * Time.deltaTime);
        IsSpin = true;
        transform.Translate(direction * spinningSpeed * Time.deltaTime);
        yield return new WaitForSeconds(5f);
        IsSpin = false;

        yield return new WaitForSeconds(1f);
    }
    public float timeWait = 2f;
    

    /*private IEnumerator FreeShotSequence(Vector2 direction)
    {
        //shootingZone.GetComponent<Collider2D>().enabled = true;
        // Trong vùng bắn
        rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, idleFriction);
        IsMinotaurMoving = false;
        Debug.Log("is aim " + isAiming);
        Debug.Log("last shoot time " + lastShootTime);
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
                KilledEnemy.bulletVector2 = direction;
                ShootArrow(direction);
                lastShootTime = Time.time;
                isAiming = false;
            }
            else
            {
                isAiming = false;  // Đảm bảo isAiming tắt nếu không bắn
            }
        }
        yield return new WaitForSeconds(10f);
        //shootingZone.enabled = false;
        //shootingZone.GetComponent<Collider2D>().enabled = false;
        isShooting = false;  // Đảm bảo isShooting tắt khi hoàn thành
    }*/

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
