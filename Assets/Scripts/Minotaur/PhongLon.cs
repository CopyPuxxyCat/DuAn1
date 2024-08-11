using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhongLon : MonoBehaviour
{
    public float phongLonDamage = 1f;
    public float knockbackForce = 10f;
    public Collider2D phongLonCollider2;



    public Vector3 faceRight = new Vector3(0.86f, -0.186f, -0.3246863f);
    public Vector3 faceLeft = new Vector3(-0.86f, -0.186f, -0.3246863f);



    private void Start()
    {
        if (phongLonCollider2 == null)
        {
            Debug.LogWarning("need to set phongLon collider!");
        }

    }
    /*void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("vua cham");
        collision.collider.SendMessage("OnHit", phongLonDamage);
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
            Vector2 direction = (Vector2)(collision.gameObject.transform.position - parentPosition).normalized;
            // knockback is in direction of phongLonCollider towards collider
            Vector2 knockback = direction * knockbackForce;
            Debug.Log("mat mau boi mina");
            // make it hit by passing the Vector2 force to the rb
            damagealeObject.OnHit(phongLonDamage, knockback);

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
