using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Enemy : MonoBehaviour
{
    [SerializeField] int health = 0;

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
