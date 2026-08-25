using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Enemy : MonoBehaviour
{
    [SerializeField] int health = 0;
    bool takingKnockback = false;
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void TakeKnockBack(float knockbackForce, Vector3 knockbackDirection)
    {
        takingKnockback = true; // to stop any logic that shouldn't run during knockback
        
        // start animation please

        if (rb != null)
        {
            rb.AddForce(knockbackDirection * knockbackForce + new Vector3(0, 0.3f, 0) * knockbackForce, ForceMode2D.Impulse);
        }
        else
        {
            GetComponent<Rigidbody2D>()?.AddForce(knockbackDirection * knockbackForce + new Vector3(0, 0.3f, 0) * knockbackForce, ForceMode2D.Impulse);
        }

        OnKnockback();

        StartCoroutine(KnockbackWaitCoroutine());
    }

    System.Collections.IEnumerator KnockbackWaitCoroutine()
    {
        float timeout = 1.0f;
        float timer = 0f;
        float stopSpeed = 0.2f;
        while (timer < timeout)
        {
            timer += Time.deltaTime;
            if (rb != null && rb.linearVelocity.magnitude <= stopSpeed)
                break;
            yield return null;
        }
        takingKnockback = false;
    }

    protected virtual void OnKnockback() 
    { 
    
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("died");
    }

}
