using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAbleEnemy : MonoBehaviour, IDamagable
{
    public GameObject healthText;

    public bool disableSimulation = false;

    public float invincibleTime = 0.25f;

    public bool isInvincibleEnable = false;

    private float invincibleElapsed = 0f;

    private int totalKill;

    Animator animator;

    Collider2D physicCollider;

    Rigidbody2D rb;

    bool isAlive = true;

    public float Health
    {
        set
        {
            if (value < _health)
            {
                animator.SetTrigger("hit");
                RectTransform textTransform = Instantiate(healthText).GetComponent<RectTransform>();
                textTransform.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);

                Canvas canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
                textTransform.SetParent(canvas.transform);
            }

            _health = value;



            if (_health <= 0)
            {
                animator.SetBool("isAlive", false);

                // tat slime
                targetAble = false;
            }
        }
        get
        {
            return _health;
        }
    }

    public bool targetAble
    {
        get { return _targetable; }
        set
        {
            _targetable = value;
            // turn of the smilated
            if (disableSimulation)
            {
                rb.simulated = false;
            }
            // turn of the collider
            physicCollider.enabled = value;
        }
    }

    public bool invincible
    {
        get { return _invincible; }
        set
        {
            _invincible = value;
            if (_invincible == true)
            {
                invincibleElapsed = 0f;
            }
        }
    }

    public bool _invincible = false;

    float _health = 3;

    bool _targetable = true;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("isAlive", isAlive);

        rb = GetComponent<Rigidbody2D>();
        physicCollider = GetComponent<Collider2D>();
    }



    public void OnHit(float damage, Vector2 knockback)
    {
        if (!invincible)
        {
            Health -= damage;

            // apply force
            rb.AddForce(knockback, ForceMode2D.Impulse);

            if (isInvincibleEnable)
            {
                invincible = true;
            }
        }
    }



    public void OnHit(float damage)
    {
        if (!invincible)
        {
            Health -= damage;
            if (isInvincibleEnable)
            {
                invincible = true;
            }
        }
    }

    public void OnObjectDestroyed()
    {
        Destroy(gameObject);
        totalKill += 1;
        Debug.Log("da giet" + totalKill);
    }

    public void FixedUpdate()
    {
        if (invincible)
        {
            invincibleElapsed += Time.deltaTime;

            if (invincibleElapsed > invincibleTime)
            {
                invincible = false;
            }
        }
    }
}
