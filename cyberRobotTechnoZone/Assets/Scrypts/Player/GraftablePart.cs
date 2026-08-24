using UnityEngine;

public class GraftablePart : MonoBehaviour
{
    bool held = false;

    void OnTriggerEnter2D(Collider2D collision) 
    {
        if (held) return;
        if(collision.transform.gameObject.CompareTag("Player"))
        {
            held = true;
            transform.position = collision.transform.position;
            this.gameObject.transform.parent = collision.transform;
        }
    }


}
