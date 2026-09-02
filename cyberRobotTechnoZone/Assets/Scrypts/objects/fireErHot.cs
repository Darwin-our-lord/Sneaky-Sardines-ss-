using UnityEngine;

public class fireErHot : MonoBehaviour
{
    bool awwww = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerManager>().TakeDamage(1);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
