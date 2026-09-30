using UnityEngine;
public class FlowerHead : MonoBehaviour
{
    FlowerBoss boss; //Remember la IsTrigger on yesyes, on a collider2D :3 (On the fucking barn of the flower boss, not the head itself)

    void Awake()
    {
        boss = GetComponentInParent<FlowerBoss>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (boss != null && collision.CompareTag("Player"))
        {
            boss.TryStomp(collision.attachedRigidbody);
        }
    }
}