using UnityEngine;

public class Fireball : MonoBehaviour
{
    Vector3 dir;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Vector3 target;
    public float speed = 1f;
    public GameObject fireprefab;
    public Vector3 offset;
    private void Awake()
    {
        offset = new Vector3(0, 2, 0);
    }
    void FixedUpdate()
    {
        Debug.Log(dir);
        if (target != null && dir.magnitude < 0.5f)
        {
            dir = (target - transform.position).normalized;
        }
        if (target != null)
        {
            transform.position = transform.position + speed * dir;
            
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerManager>().TakeDamage(1);
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == 3)
        {
            Instantiate(fireprefab, transform.position + offset, Quaternion.Euler(0, 0, 0));
            Destroy(gameObject);
        }
    }
}
