using UnityEngine;

public class ParticleFollowPlayer2D : MonoBehaviour
{
    public Transform player;
    public Transform particleSystemTransform;
    void Update()
    {
        Vector3 direction = player.position - particleSystemTransform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        particleSystemTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}