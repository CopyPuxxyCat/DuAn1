using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D rb;
    DamageableCharacter damageableCharacter;


    private void Start()
    {
        rb.GetComponent<Rigidbody2D>();
    }
}
