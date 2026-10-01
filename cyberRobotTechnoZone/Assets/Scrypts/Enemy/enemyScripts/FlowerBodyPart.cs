using UnityEngine;
public class FlowerBodyPart : MonoBehaviour
{
    FlowerBoss boss;

    void Awake()
    {
        boss = GetComponentInParent<FlowerBoss>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (boss != null) boss.HandleBodyContact(collision.collider);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (boss != null) boss.HandleBodyContact(collision.collider);
    }
}