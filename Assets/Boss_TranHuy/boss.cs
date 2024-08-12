using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class boss : MonoBehaviour
{
    public Animator ani;
    public GameObject Players,lazer,da;
    public float dichuyenphe;
    public int dichuyen = 0;
    public Vector3 move;
    public Transform nem;
    public Rigidbody2D rb;

    public Slider thanhmau_Boss;
    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
        dichuyenphe = transform.position.x;
        StartCoroutine(hoisinh());
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        float dichuyenboss = transform.position.x;
        if(dichuyenboss < dichuyenphe)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if(dichuyenboss > dichuyenphe)
        {
            transform.localScale = new Vector3(1,1,1);
        }
        dichuyenphe = dichuyenboss;
        if (dichuyen == 1)
        {
            move = Players.transform.position - transform.position;
            transform.position += move * 1f * Time.deltaTime;
        }
        thanhmau_Boss.value = KilledEnemy.boss_health_Manager;
    }
    IEnumerator hoisinh()
    {
        yield return new WaitForSeconds(2f);
        dichuyen++;
        ani.SetBool("run",true);
        yield return new WaitForSeconds(3f);
        ani.SetBool("fire",true);
        lazer.SetActive(true);
        yield return new WaitForSeconds(5f);
        dichuyen++;
        ani.SetBool("hetnangluong", true);
        yield return new WaitForSeconds(6f);
        while(dichuyen > 1)
        {
            dichuyen = 0;
        }
        yield return new WaitForSeconds(2f);
        dichuyen++;
        ani.SetBool("nemda", true);
        yield return new WaitForSeconds(1f);
        ani.SetBool("nemda",false);
        ani.SetBool("hetnangluong",false );
        ani.SetBool("fire", false);
        ani.SetBool("run", true);
        move = Players.transform.position - transform.position;
        Vector3 huong = move - transform.position;
        GameObject go = Instantiate(da, nem.position, nem.rotation);
        Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
        rb.AddForce(nem.right * 2f * Mathf.Sign(transform.localScale.x), ForceMode2D.Impulse);
        yield return new WaitForSeconds(1f);
        ani.SetBool("chet", true);
        ani.SetBool("run", false);
        dichuyen++;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Collider2D collider = col.collider;
        IDamagable damageable = collider.GetComponent<IDamagable>();

        if (damageable != null)
        {
            Vector2 direction = (collider.transform.position - transform.position).normalized;
            Vector2 knockback = direction * 2f;
            damageable.OnHit(1f, knockback);
        }
    }
}
