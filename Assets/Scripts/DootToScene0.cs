using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DootToScene0 : MonoBehaviour
{
    private bool enterAllowed;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(WaitToActive());
        }
    }

    IEnumerator WaitToActive()
    {
        yield return new WaitForSeconds(10f);
        // Thực hiện hành động ở đây
        enterAllowed = true;
        KilledEnemy.sharedValue = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (enterAllowed == true)
        {
            SceneManager.LoadScene("Level0");

        }
    }
}
