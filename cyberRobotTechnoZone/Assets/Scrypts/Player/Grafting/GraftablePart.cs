using UnityEngine;

public class GraftablePart : MonoBehaviour
{
    protected bool held = false; //to stop checking for pickup when already held

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision detected with: " + collision.transform.gameObject.name);
        if (held) return;
        if (collision.transform.gameObject.CompareTag("Player"))
        {
            held = true;
            transform.position = collision.transform.position;
            transform.SetParent(collision.transform);
            OnAttach();
        }
    }

    protected virtual void OnAttach()
    {
        // override in subclasses
    }

    // Detach the part and call OnDetach hook.
    public virtual void Detach()
    {
        if (!held) return;
        held = false;
        transform.SetParent(null);
        OnDetach();
    }

    protected virtual void OnDetach()
    {
        // override in subclasses
    }
}
