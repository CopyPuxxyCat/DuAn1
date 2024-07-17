using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    public float speed = 2f;  // Movement speed of the slime
    public float chaseRange = 4f; // Range at which the slime chases the player
    public float idleRange = 2f;   // Range at which the slime starts idling

    private Transform player;   // Reference to the player transform
    private Vector2 targetPosition; // Target position for movement

    void Start()
    {
        player = GameObject.Find("Player").transform; // Find the player object at runtime
        targetPosition = transform.position; // Set initial target position to current position
    }

    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer <= chaseRange) // Chase player if within chase range
            {
                targetPosition = player.position;
            }
            else if (distanceToPlayer > chaseRange && distanceToPlayer > idleRange) // Random movement if outside chase range but within idle range
            {
                targetPosition = GetRandomPosition();
            }
            // Stay still if the player is outside both chase and idle range

            // Move the slime towards the target position
            Vector2 direction = new Vector2(targetPosition.x - transform.position.x, targetPosition.y - transform.position.y);
            direction.Normalize();
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }

    Vector2 GetRandomPosition()
    {
        // Define the area for random movement (adjust based on your scene size)
        float randomX = Random.Range(transform.position.x - 5f, transform.position.x + 5f);
        float randomY = Random.Range(transform.position.y - 5f, transform.position.y + 5f);
        return new Vector2(randomX, randomY);
    }
}
