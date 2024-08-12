

using UnityEngine;

public interface IDamagable
{
    public float PlayerHealth { get; set; }
    public float EnemyHealth { get; set; }
    public float BossHealth { get; set; }
    public bool targetAble { get; set; }

    public bool invincible { get; set; }
    public void OnHit(float damage, Vector2 knockback);
    public void OnHit(float damage);

    public void OnObjectDestroyed();



}
