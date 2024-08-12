using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DamageableCharacter : MonoBehaviour, IDamagable
{
    public GameObject healthText;

    //public GameObject gameObject;

    public bool disableSimulation = false;

    public float invincibleTime = 0.25f;

    public bool isInvincibleEnable = false;

    private float invincibleElapsed = 0f;

    private int kill;
    private int totalKill;

    Animator animator;

    Collider2D physicCollider;

    Rigidbody2D rb;
    public

    bool isAlive = true;

    // health
    public float player_health = 10f; // Health cho player
    public float enemy_health = 3f;  // Health cho enemy
    public float boss_health = 10f;  // Health cho boss

    public GameObject GameOver;

    public bool _invincible = false;

    //public float _health = 3;
    //public GameObject[] LivesImage;


    bool _targetable = true;

    /*public float Health
    {
        set
        {
            if (gameObject.CompareTag("Player"))
            {
                if (value < player_health)
                {
                    animator.SetTrigger("hit");
                    RectTransform textTransform = Instantiate(healthText).GetComponent<RectTransform>();
                    textTransform.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
                    //Debug.Log("play mat 1 mau");
                    Canvas canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
                    textTransform.SetParent(canvas.transform);
                }

                player_health = value;

                if (player_health <= 0)
                {
                    animator.SetBool("isAlive", false);
                    targetAble = false;
                    // hien panel gameover
                    Time.timeScale = 0f;
                    GameOver.SetActive(true);
                    KilledEnemy.sharedValue = 0;
                }
            }
            else if (gameObject.CompareTag("Enemy"))
            {
                if (value < enemy_health)
                {
                    animator.SetTrigger("hit");
                    RectTransform textTransform = Instantiate(healthText).GetComponent<RectTransform>();
                    textTransform.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
                    //Debug.Log("quai mat 1 mau");
                    Canvas canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
                    textTransform.SetParent(canvas.transform);
                }

                enemy_health = value;

                if (enemy_health <= 0)
                {
                    animator.SetBool("isAlive", false);
                    targetAble = false;
                }
            }
            else if (gameObject.CompareTag("Boss"))
            {
                if (value < enemy_health)
                {
                    animator.SetTrigger("hit");
                    RectTransform textTransform = Instantiate(healthText).GetComponent<RectTransform>();
                    textTransform.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);

                    Canvas canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
                    textTransform.SetParent(canvas.transform);
                }

                enemy_health = value;

                if (enemy_health <= 0)
                {
                    animator.SetBool("isAlive", false);
                    targetAble = false;
                }
            }
        }
        get
        {
            if (gameObject.CompareTag("Player"))
            {
                //KilledEnemy.player_health_Manager = player_health;
                return player_health;
            }
            else if (gameObject.CompareTag("Enemy"))
            {
                return enemy_health;
            }
            else if (gameObject.CompareTag("Boss"))
            {
                //KilledEnemy.player_health_Manager = boss_health;
                return boss_health;
            }
            return 0f;
        }
    }*/

    public float PlayerHealth
    {
        set
        {
            if (gameObject.CompareTag("Player"))
            {
                if (value < player_health)
                {
                    animator.SetTrigger("hit");
                    RectTransform textTransform = Instantiate(healthText).GetComponent<RectTransform>();
                    textTransform.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
                    Canvas canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
                    textTransform.SetParent(canvas.transform);
                }

                player_health = value;

                if (player_health <= 0)
                {
                    animator.SetBool("isAlive", false);
                    targetAble = false;
                    Time.timeScale = 0f;
                    GameOver.SetActive(true);
                    KilledEnemy.sharedValue = 0;
                }
            }
        }
        get
        {
            if (gameObject.CompareTag("Player"))
            {
                KilledEnemy.player_health_Manager = player_health;
                return player_health;
            }
            else
            {
            return 0f;
        }
    }
    }

    public float EnemyHealth
    {
        set
        {
            if (gameObject.CompareTag("Enemy"))
            {
                if (value < enemy_health)
                {
                    animator.SetTrigger("hit");
                    RectTransform textTransform = Instantiate(healthText).GetComponent<RectTransform>();
                    textTransform.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
                    Canvas canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
                    textTransform.SetParent(canvas.transform);
                }

                enemy_health = value;

                if (enemy_health <= 0)
                {
                    animator.SetBool("isAlive", false);
                    targetAble = false;
                }
            }
        }
        get
        {
            if (gameObject.CompareTag("Enemy"))
            { 
                KilledEnemy.enemy_health_manager = enemy_health;
                return enemy_health;
            }
            else
            {
                return 0f;
            }
        }
    }
    

    public float BossHealth
    {
        set
        {
            if (gameObject.CompareTag("Boss"))
            {
                if (value < boss_health)
                {
                    animator.SetTrigger("hit");
                    RectTransform textTransform = Instantiate(healthText).GetComponent<RectTransform>();
                    textTransform.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
                    Canvas canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
                    textTransform.SetParent(canvas.transform);
                }

                boss_health = value;

                if (boss_health <= 0)
                {
                    KilledEnemy.isMinotaurKilled = true;
                    animator.SetBool("isAlive", false);
                    targetAble = false;
                }
            }
        }
        get
        {
            if (gameObject.CompareTag("Boss"))
            {
                KilledEnemy.boss_health_Manager = boss_health;
                return boss_health;
            }
            else
            {
                return 0f;
            }
        }
    }

    // Health cu
    /*public float Health
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
                // hien panel gameover
                //Time.timeScale = 0f;
                //GameOver.SetActive(true);
            }
        }
        get
        {
            return _health;
        }
    }*/

    public bool targetAble
    {
        get { return _targetable; }
        set
        {
            _targetable = value;
            // turn of the smilated
            if(disableSimulation)
            {
                rb.simulated = false;
            }
            // turn of the collider
            physicCollider.enabled = value;
        }
    }

    public bool invincible { get { return _invincible; }
        set { _invincible = value;
        if(_invincible == true)
            {
                invincibleElapsed = 0f;
            }
        }
    }

    

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("isAlive", isAlive);

        rb = GetComponent<Rigidbody2D>();
        physicCollider = GetComponent<Collider2D>();

    }



    public void OnHit(float damage, Vector2 knockback)
    {
       if( !invincible)
        {
            /*Health -= damage;

            // apply force
            rb.AddForce(knockback, ForceMode2D.Impulse);

            if(isInvincibleEnable)
            {
                invincible = true;
            }*/

            if (gameObject.CompareTag("Player"))
            {
                PlayerHealth -= damage;

                // apply force
                rb.AddForce(knockback, ForceMode2D.Impulse);

                if (isInvincibleEnable)
                {
                    invincible = true;
                }
            }
            else if (gameObject.CompareTag("Enemy"))
            {
                EnemyHealth -= damage;

                // apply force
                rb.AddForce(knockback, ForceMode2D.Impulse);
            }
            else if (gameObject.CompareTag("Boss"))
            {
                BossHealth -= damage;

                // apply force
                rb.AddForce(knockback, ForceMode2D.Impulse);
            }
        }
    }



    public void OnHit(float damage)
    {
        if (!invincible)
        {
            /*Health -= damage;

            // apply force
            rb.AddForce(knockback, ForceMode2D.Impulse);

            if(isInvincibleEnable)
            {
                invincible = true;
            }*/

            if (gameObject.CompareTag("Player"))
            {
                PlayerHealth -= damage;

                // apply force


                if (isInvincibleEnable)
                {
                    invincible = true;
                }
            }
            else if (gameObject.CompareTag("Enemy"))
            {
                EnemyHealth -= damage;
            }
            else if (gameObject.CompareTag("Boss"))
            {
                BossHealth -= damage;
            }
        }
    }

    public void OnObjectDestroyed()
    {
        if (gameObject.CompareTag("Enemy"))
        {
            kill = 1;
            KilledEnemy.sharedValue += kill;
            KilledEnemy.enemyVector3 = gameObject.transform.position;
        }
        /*if (gameObject.CompareTag("Boss"))
        {
            KilledEnemy.isMinotaurKilled = true;
        }*/
        Destroy(gameObject);
    }

    /*public int TotalKilled()
    {
        Debug.Log("KilledEnemy.sharedValue" + KilledEnemy.sharedValue);
        totalKill = kill;
        Debug.Log("da giet tong cong: " + KilledEnemy.sharedValue);
        return totalKill;
    }*/

    
    private void Update()
    {
        //TotalKilled();

        /*for (int i = 0; i < player_health; i++)
        {
            if (i < _health)
            {
                LivesImage[i].SetActive(true);
            }
            else
            {
                LivesImage[i].SetActive(false);
            }
        }*/

        Debug.Log("mau quai dc: " + BossHealth);
    }

    public void FixedUpdate()
    {
        if (invincible)
        {
            invincibleElapsed += Time.deltaTime;

            if(invincibleElapsed > invincibleTime)
            {
                invincible = false;
            }
        }
    }
}
