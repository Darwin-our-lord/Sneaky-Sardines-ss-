using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public List<Sprite> headsprite;
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
        if (health <= 0) { Die(); }
        GetComponent<SpriteRenderer>().sprite = headsprite[health];


    }

    public void Die()
    {
        // Handle player death (e.g., reload scene, show game over screen)
        Debug.Log("Player has died.");
    }

}
