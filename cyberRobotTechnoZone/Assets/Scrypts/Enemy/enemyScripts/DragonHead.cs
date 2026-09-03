using UnityEngine;

public class DragonHead : Enemy
{
    public override void Die()
    {
        dragonEnemy dragon = GetComponentInParent<dragonEnemy>();
        dragon.Die();
        Debug.LogWarning("dragonhead dead");
    }
}
