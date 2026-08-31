using UnityEngine;

public class PlantWhip : GraftablePart
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!held && collision.transform.gameObject.CompareTag("Player"))
        {
            held = true;
            transform.position = collision.transform.position;
            collision.gameObject.GetComponent<LeafAttack>().held = true; // Set the static variable to true when the whip is picked up
            Destroy(this.gameObject);
        }
    }
}
