using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    int health = 3;
    [SerializeField] private int maxHealth = 3;

    public void Heal(int amount)
    {
        health += amount;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        // Handle player death (e.g., reload scene, show game over screen)
        Debug.Log("Player has died.");
    }

}
