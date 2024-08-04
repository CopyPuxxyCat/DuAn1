using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swordHitBox : MonoBehaviour
{
    public float swordDamage = 1f;
    public float knockbackForce = 4f;
    public Collider2D swordCollider2;

    

    public Vector3 faceRight = new Vector3(0.4f, -0.2f,0);
    public Vector3 faceLeft = new Vector3(-0.4f, -0.2f, 0);


    private void Start()
    {
        if (swordCollider2 == null)
        {
            Debug.LogWarning("need to set sword collider!");
        }
        
    }
    /*void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("vua cham");
        collision.collider.SendMessage("OnHit", swordDamage);
    }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamagable damagealeObject = collision.GetComponent<IDamagable>();

        // calculate direction between character and slime
        if (damagealeObject != null)
        {
            //Calculate direction between character and slime
            Vector3 parentPosition = transform.parent.position;

            // offset for collision change the direction where the force come from (close to the player)
            Vector2 direction = (Vector2)( collision.gameObject.transform.position - parentPosition).normalized;
            // knockback is in direction of swordCollider towards collider
            Vector2 knockback = direction * knockbackForce;

            // make it hit by passing the Vector2 force to the rb
            damagealeObject.OnHit(swordDamage, knockback);
            
        }
        else
        {
            
        }

    }

    

    void isFacingRight(bool isFacingRight)
    {
        if (isFacingRight)
        {
            gameObject.transform.localPosition = faceRight;
        }
        else
        {
            gameObject.transform.localPosition = faceLeft;
        }
    }

    
}
