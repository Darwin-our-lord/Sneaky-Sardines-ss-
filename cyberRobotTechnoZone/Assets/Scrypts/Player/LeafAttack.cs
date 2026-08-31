using Unity.GraphToolkit.Editor;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class LeafAttack : MonoBehaviour
{
    public static bool held;
    [SerializeField] Animator animator;
    [SerializeField] AudioClip attackSound; // the sound of the whip attack 
    [SerializeField] AudioClip attackHitSound; // the sound of the whip hitting an enemy

    private void Update()
    {
        if (held && Input.GetKeyDown(KeyCode.Mouse0))
        {
            // Perform whip attack
            animator.SetTrigger("WhipAttack");
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
