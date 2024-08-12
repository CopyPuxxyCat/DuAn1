using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class binhMauManager : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(KilledEnemy.TongSoBinhMau < 10)
            {
                KilledEnemy.TongSoBinhMau++;
            }
            Destroy(gameObject);
        }
    }
}
