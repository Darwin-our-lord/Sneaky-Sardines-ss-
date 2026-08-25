using UnityEngine;

public class PlantWhip : GraftablePart
{
    Animator animator;
    protected override void OnAttach()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (held && Input.GetKeyDown(KeyCode.Mouse0))
        {
            // Perform whip attack
            animator.SetTrigger("Attack");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (held && collision.gameObject.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().TakeDamage(1);// Perform damage to the enemy
        }
    }
}
