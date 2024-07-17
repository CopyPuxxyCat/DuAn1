using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    public float speed = 2f;  // Movement speed of the slime
    public Transform player;   // Reference to the player transform

    void Start()
    {
        player = GameObject.Find("Player").transform; // Find the player object at runtime
    }

    void Update()
    {
        if (player != null)
        {
            // Calculate the direction vector from the slime to the player
            Vector2 direction = player.position - transform.position;

            // Normalize the direction vector to get a unit vector
            direction.Normalize();

            // Move the slime towards the player based on speed and direction
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }
}
