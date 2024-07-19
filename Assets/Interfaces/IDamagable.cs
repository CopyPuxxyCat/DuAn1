

using UnityEngine;

public interface IDamagable
{
    public float Health { get; set; }
    public bool targetAble { get; set; }

    public bool invincible { get; set; }
    public void OnHit(float damage, Vector2 knockback);
    public void OnHit(float damage);

    public void OnObjectDestroyed();



}
