using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Enemy : MonoBehaviour
{
    [SerializeField] int health = 0;
    bool takingKnockback = false;
    public void TakeKnockBack(float knockbackForce, Vector3 knockbackDirection)
    {
        takingKnockback = true; //to stop any logic that shouldnt run during knockback
        //start animation
        this.GetComponent<Rigidbody2D>().AddForce
            (knockbackDirection * knockbackForce + new Vector3(0, 0.3f, 0) * knockbackForce, ForceMode2D.Impulse); //apply knockback force

        while (knockbackForce > 0) { } //wait until knockback is done (might not be best way to do this:o)
        takingKnockback = false;
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
