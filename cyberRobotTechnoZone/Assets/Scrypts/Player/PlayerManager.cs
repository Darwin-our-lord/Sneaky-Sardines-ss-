using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] MenuManager menuManager;
    [SerializeField] Animator deathAnimator;
    [SerializeField] Animator animator;
    [SerializeField] int health = 3;
    [SerializeField] private int maxHealth = 3;


    float invincibilityDuration = 0.5f; // Duration of invincibility in seconds
    float lastDmgTime = 0.0f;

    [SerializeField] AudioClip HurtSound; // The sound of the player getting hurt

    [Header("Head Sprites")]
    [SerializeField] List<Sprite> headSpritesNormal;
    [SerializeField] List<Sprite> headSpritesUp;
    [SerializeField] List<Sprite> headSpritesDown;
    [SerializeField] GameObject upPlayerHeadObj;
    [SerializeField] GameObject downPlayerHeadObj;
    [SerializeField] GameObject normalPlayerHeadObj;
    public void Heal(int amount)
    {
        health += amount;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }
    void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
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
        if (animator != null)
            {
                animator.SetTrigger("Hurt");
            }
        if (health <= 0) { StartCoroutine(Die()); }


        if (normalPlayerHeadObj != null && headSpritesNormal[health - 1] != null)
        {
            normalPlayerHeadObj.GetComponent<SpriteRenderer>().sprite = headSpritesNormal[health-1];
        }
        if (upPlayerHeadObj != null && headSpritesUp[health - 1] != null)
        {
            upPlayerHeadObj.GetComponent<SpriteRenderer>().sprite = headSpritesUp[health-1];
        }
        if (downPlayerHeadObj != null && headSpritesDown[health - 1] != null)
        {
            downPlayerHeadObj.GetComponent<SpriteRenderer>().sprite = headSpritesDown[health-1];
        }
    }

    IEnumerator Die()
    {
        normalPlayerHeadObj.SetActive(false);
        upPlayerHeadObj.SetActive(false);
        downPlayerHeadObj.SetActive(false);

        deathAnimator.SetTrigger("Death");
        yield return new WaitForSeconds(3f);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
