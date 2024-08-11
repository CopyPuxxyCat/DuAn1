using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class Shootenemy : MonoBehaviour
{
    public int timers = 0;
    public GameObject bullet;
    public Transform shootpoint;
    public Transform players;
    public Transform player;
    //public Transform enemypoint;
    //public float health = 100f;
    public float range = 5f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(shootting());
    }

    // Update is called once per frame
    void Update()
    {
        float position = Vector3.Distance(player.position, transform.position);
        if (position <= range)
        {
            Vector3 p = player.position - transform.position;
            transform.position += p * 1f * Time.deltaTime;
            if (timers == 0)
            {
                Vector2 shoot = player.position - transform.position;
                GameObject go = Instantiate(bullet, shootpoint.position, shootpoint.rotation);
                Rigidbody2D rp = go.GetComponent<Rigidbody2D>();
                rp.velocity += shoot * 5f;
                Destroy(rp.gameObject, 1f);
            }
        }
        //else if (position > range)
        //{
        //    Vector3 pp = enemypoint.position - transform.position;
        //    transform.position += pp * 1f * Time.deltaTime;
        //}
        //if (health <= 0f)
        //{
        //    Destroy(this.gameObject);
        //}
    }
    IEnumerator shootting()
    {
        while (timers == 0)
        {
            timers += 1;
            yield return new WaitForSeconds(3f);
            while (timers == 1)
            {
                timers -= 1;
                yield return new WaitForSeconds(0.0005f);
            }
        }
    }
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("bullet"))
    //    {
    //        health -= 50f;
    //        Destroy(collision.gameObject);
    //    }
    //}
}