using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    bool IsSlimeMoving
    {
        set
        {
            isSlimeMoving = value;
            animator.SetBool("isSlimeMoving", isSlimeMoving);
        }
    }

    public float damage = 1;

    public float knockbackForce = 2f;

    public float moveSpeed = 50f;

    public DetectionZone detectionZone;

    public Rigidbody2D rb;

    public float idleFriction = 0.9f;

    Animator animator;

    bool isSlimeMoving = false;

    DamageableCharacter damageableCharacter;

    private void Start()
    {
        rb.GetComponent<Rigidbody2D>();
        damageableCharacter = GetComponent<DamageableCharacter>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (damageableCharacter.targetAble && detectionZone.detectedObjs.Count > 0)
        {
            // calculate direction to target
            Vector2 direction = (detectionZone.detectedObjs[0].transform.position - transform.position).normalized;
            // move toward the objs
            rb.AddForce(direction * moveSpeed * Time.deltaTime);
            IsSlimeMoving = true;
        }
        else
        {
            // no movement so interpolate velocity toward 0
            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, idleFriction);

            IsSlimeMoving = false;
        }
    }

    // damage slime do to smth
    void OnCollisionEnter2D(Collision2D col)
    {
        Collider2D collider = col.collider;
        IDamagable damageable = collider.GetComponent<IDamagable>();

        if (damageable != null)
        {
            //Debug.Log("mat mau");
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
    

}
