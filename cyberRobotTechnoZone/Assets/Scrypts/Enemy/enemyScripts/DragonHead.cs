using UnityEngine;

public class DragonHead : Enemy
{
    bool isStunned = false;
    public override void TakeDamage(int damage)
    {
        if (!isStunned) return;
        base.TakeDamage(damage);
    }
    public override void Die()
    {
        dragonEnemy dragon = GetComponentInParent<dragonEnemy>();
        dragon.Die();
        Debug.LogWarning("dragonhead dead");
    }
}
