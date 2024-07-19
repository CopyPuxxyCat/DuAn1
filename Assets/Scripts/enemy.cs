using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    public float speed = 2f;  // Movement speed of the slime
    public float chaseRange = 4f; // Range at which the slime chases the player

    private Transform player;   // Reference to the player transform
    private Vector2 targetPosition; // Target position for movement

    void Start()
    {
        player = GameObject.Find("Player").transform; // Find the player object at runtime
        targetPosition = transform.position; // Set initial target position to current position
    }

    void Update()
    {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer <= chaseRange) // Chase player if within chase range
            {
                targetPosition = player.position;
            }
            Vector2 direction = new Vector2(targetPosition.x - transform.position.x, targetPosition.y - transform.position.y);
            direction.Normalize();
            transform.Translate(direction * speed * Time.deltaTime);
        }

}
