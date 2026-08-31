using UnityEditor;
using UnityEngine;

public class LeafAttack : MonoBehaviour
{
    public bool held = false;

    [SerializeField] GameObject leaf;
    [SerializeField] Animator animator;
    [SerializeField] AudioClip attackSound; // the sound of the whip attack 
    [SerializeField] AudioClip attackHitSound; // the sound of the whip hitting an enemy

    public void LateUpdate()
    {
        if (held)
        {
            leaf.GetComponent<SpriteRenderer>().enabled = true;
        }
    }
    void Update()
    {
        if (held && Input.GetKeyDown(KeyCode.Mouse0))
        {
            // Perform whip attack
            animator.SetTrigger("Attack");
            AudioSource.PlayClipAtPoint(attackSound, transform.position, 1f);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (held && collision.gameObject.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().TakeDamage(1);// Perform damage to the enemy
            AudioSource.PlayClipAtPoint(attackHitSound, transform.position, 1f);
        }
    }
}
