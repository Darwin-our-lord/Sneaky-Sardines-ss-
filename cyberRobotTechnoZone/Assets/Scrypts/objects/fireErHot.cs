using System.Collections;
using UnityEngine;

public class fireErHot : MonoBehaviour
{
    bool awwww = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        StartCoroutine(TimerDeath());
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerManager>().TakeDamage(1);
        }
    }
    IEnumerator TimerDeath()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
