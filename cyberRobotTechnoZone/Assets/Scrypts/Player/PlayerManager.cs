using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] List<Sprite> headsprite;
    [SerializeField] GameObject playerHeadObj;
    [SerializeField] int health = 3;
    [SerializeField] private int maxHealth = 3;
    float invincibilityDuration = 0.5f; // Duration of invincibility in seconds
    float lastDmgTime = 0.0f;

    [SerializeField] AudioClip HurtSound; // The sound of the player getting hurt
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
        invincibilityDuration -= Time.time - lastDmgTime;
        if (invincibilityDuration > 0f) 
        {
            return; // Ignore damage while invincible
        }
        lastDmgTime = Time.time;
        health -= damage;
        AudioSource.PlayClipAtPoint(HurtSound, transform.position, 1000f);
        if (health <= 0) { Die(); }
        if (playerHeadObj != null && headsprite[health] != null)
        {
            playerHeadObj.GetComponent<SpriteRenderer>().sprite = headsprite[health];
        }
    }

    public void Die()
    {
        // Handle player death (e.g., reload scene, show game over screen)
        Debug.Log("Player has died.");
    }

}
