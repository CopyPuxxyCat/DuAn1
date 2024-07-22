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
            enterAllowed = true;
            Debug.Log("Cham");
            // Load a new scene when the player enters the door

        }
    }
    // Start is called before the first frame update
    void Start()
    {

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
