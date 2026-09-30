using UnityEngine;
public class FlowerArenaTrigger : MonoBehaviour
{
    [SerializeField] FlowerBoss boss;
    [SerializeField] GameObject[] enableOnStart; // arena walls n shit
    bool started = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (started || !collision.CompareTag("Player")) return;
        started = true;

        foreach (GameObject obj in enableOnStart) if (obj != null) obj.SetActive(true);
        if (boss != null) boss.StartFight();
    }
}