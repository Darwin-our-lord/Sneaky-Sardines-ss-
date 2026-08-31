using UnityEngine;

public class PlantWhip : GraftablePart
{

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!held && collision.transform.gameObject.CompareTag("Player"))
        {
            held = true;

            LeafAttack.held = true;

            Destroy(gameObject);
        }
    }
}
