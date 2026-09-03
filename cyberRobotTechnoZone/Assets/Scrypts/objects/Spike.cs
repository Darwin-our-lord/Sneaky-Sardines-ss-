using UnityEngine;

public class Spike : MonoBehaviour
{
    int damage = 10;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerManager>().TakeDamage(damage);
        }
    }
}
