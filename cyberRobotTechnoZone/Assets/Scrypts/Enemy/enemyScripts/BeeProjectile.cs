using System.Collections;
using UnityEngine;

public class BeeProjectile : MonoBehaviour
{
    GameObject player;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(DestroyAfterTime(5f));
    }
    private void FixedUpdate()
    {
        if (player != null)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            float speed = 2f;
            transform.position += direction * speed * Time.deltaTime + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0f);
            transform.rotation = Quaternion.LookRotation(Vector3.forward, direction); // Rotate the projectile to face the player
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerManager>().TakeDamage(1);
        }

        Destroy(this.gameObject);
    }
    IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }
}
