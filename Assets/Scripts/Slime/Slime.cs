using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    public float damage = 1;

    public float knockbackForce = 2f;

    public float moveSpeed = 50f;

    public DetectionZone detectionZone;

    public Rigidbody2D rb;

    DamageableCharacter damageableCharacter;

    private void Start()
    {
        rb.GetComponent<Rigidbody2D>();
        damageableCharacter = GetComponent<DamageableCharacter>();
    }

    private void FixedUpdate()
    {
        if (damageableCharacter.targetAble && detectionZone.detectedObjs.Count > 0)
        {
            // calculate direction to target
            Vector2 direction = (detectionZone.detectedObjs[0].transform.position - transform.position).normalized;
            // move toward the objs
            rb.AddForce(direction * moveSpeed * Time.deltaTime);
        }
    }

    // damage slime do to smth
    void OnCollisionEnter2D(Collision2D col)
    {
        Collider2D collider = col.collider;
        IDamagable damageable = collider.GetComponent<IDamagable>();

        if (damageable != null)
        {
            Debug.Log("mat mau");
            //Calculate direction between character and slime
            //Vector3 parentPosition = transform.parent.position;

            // offset for collision change the direction where the force come from (close to the player)
            Vector2 direction = (collider.transform.position - transform.position).normalized;
            // knockback is in direction of swordCollider towards collider
            Vector2 knockback = direction * knockbackForce;

            // make it hit by passing the Vector2 force to the rb
            damageable.OnHit(damage, knockback);
        }
    }
    /* void OnCollisionEnter2D(Collision2D collision)
    {
        IDamagable damageable = collision.collider.GetComponent<IDamagable>();

        if(damageable != null)
                {
            damageable.OnHit(damage);
        }
    }*/

}
