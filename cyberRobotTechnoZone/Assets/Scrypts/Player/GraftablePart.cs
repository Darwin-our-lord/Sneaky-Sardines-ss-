using UnityEngine;

public class GraftablePart : MonoBehaviour
{
    bool held = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (held) return;
        if (collision.transform.gameObject.CompareTag("Player"))
        {
            held = true;
            transform.position = collision.transform.position;
            transform.SetParent(collision.transform);
            OnAttach();
        }
    }

    // Hook for derived parts to run initialization when attached.
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
