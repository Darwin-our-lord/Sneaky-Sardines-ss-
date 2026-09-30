using UnityEngine;

public class GraftablePart : MonoBehaviour
{
    protected bool held = false; //to stop checking for pickup when already held
    [SerializeField] protected GameObject HeldOBJ;

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!held && collision.transform.gameObject.CompareTag("Player"))
        {
            if (HeldOBJ == null) Debug.LogError(this.gameObject.name + " - is missing its held obj guys, yall should really get around to fixing that");
            GameObject gh = Instantiate(HeldOBJ);
            gh.transform.position = collision.transform.position;
            gh.GetComponent<GraftablePart>().held = true;
            gh.transform.SetParent(collision.transform);
            gh.GetComponent<GraftablePart>().OnAttach();
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
