using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    public float bulletDamage = 1f;
    public float knockbackForce = 2f;
    public Collider2D bulletCollider2;
    Rigidbody2D rb;


    private void Start()
    {
        rb.GetComponent<Rigidbody2D>();
        if (bulletCollider2 == null)
        {
            Debug.LogWarning("need to set bullet collider!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamagable damagealeObject = collision.GetComponent<IDamagable>();

        // calculate direction between character and slime
        if (damagealeObject != null)
        {
            //Calculate direction between character and slime
            Vector3 parentPosition = transform.parent.position;

            // offset for collision change the direction where the force come from (close to the player)
            Vector2 direction = (Vector2)(collision.gameObject.transform.position - parentPosition).normalized;
            // knockback is in direction of bulletCollider towards collider
            Vector2 knockback = direction * knockbackForce;

            // make it hit by passing the Vector2 force to the rb
            damagealeObject.OnHit(bulletDamage, knockback);

        }
        else
        {

        }

    }

}
