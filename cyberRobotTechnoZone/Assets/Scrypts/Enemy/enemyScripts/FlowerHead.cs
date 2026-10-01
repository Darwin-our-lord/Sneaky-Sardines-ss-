using UnityEngine;

// trigger collider on the boss's head, don't tag it Enemy
public class FlowerHead : MonoBehaviour
{
    FlowerBoss boss;

    void Awake()
    {
        boss = GetComponentInParent<FlowerBoss>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Check(collision);
    }

    // catches the player flying up through the head and then falling back down
    private void OnTriggerStay2D(Collider2D collision)
    {
        Check(collision);
    }

    void Check(Collider2D collision)
    {
        if (boss != null && collision.CompareTag("Player"))
        {
            boss.TryStomp(collision.attachedRigidbody);
        }
    }
}