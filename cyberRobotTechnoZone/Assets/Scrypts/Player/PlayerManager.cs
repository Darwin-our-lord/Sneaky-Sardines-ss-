using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] List<Sprite> headsprite;
    [SerializeField] GameObject playerHeadObj;
    [SerializeField] MenuManager menuManager;
    [SerializeField] Animator deathAnimator;
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
        if (health <= 0) { StartCoroutine(Die()); }
        if (playerHeadObj != null && headsprite[health] != null)
        {
            playerHeadObj.GetComponent<SpriteRenderer>().sprite = headsprite[health];
        }
    }

    IEnumerator Die()
    {
        deathAnimator.SetTrigger("Death");
        yield return new WaitForSeconds(3f);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
