using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class RespawnScript : MonoBehaviour
{
    public float respawnTime = 5f; // Thời gian chờ trước khi tái tạo
    private Vector3 spawnPosition;
    public GameObject puppetPrefab;
    public Rigidbody2D rb;
    Collider2D physicCollider;

    private void OnDestroy()
    {
        spawnPosition = transform.position;
        
        StartCoroutine(Respawn());
    }

    private void Start()
    {
        rb.GetComponent<Rigidbody2D>();
    }

        IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTime);
        rb.simulated = true;
        physicCollider.enabled = true;
        Instantiate(puppetPrefab, spawnPosition, Quaternion.identity);
    }
}
