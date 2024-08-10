using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhongLonBay : MonoBehaviour
{
    // Start is called before the first frame update
    public float bulletDamage = 1f;
    public float knockbackForce = 2f;
    public Collider2D bulletCollider2;
    public float timeToWait = 0.3f;
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (bulletCollider2 == null)
        {
            Debug.LogWarning("need to set bullet collider!");
        }

        StartCoroutine(WaitAfterShoot());
    }

    IEnumerator WaitAfterShoot()
    {
        animator.SetTrigger("bulletHit");
        yield return new WaitForSeconds(timeToWait);
        // Thực hiện hành động ở đây
        bulletCollider2.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
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
        else
        {

        }
    }
}
