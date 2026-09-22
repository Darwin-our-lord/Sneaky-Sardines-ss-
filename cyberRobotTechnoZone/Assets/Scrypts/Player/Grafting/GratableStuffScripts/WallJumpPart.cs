using UnityEngine;

public class WallJumpPart : GraftablePart
{
    PlayerMovement playerMovement;
    Rigidbody rb;

    public bool touchingWall;
    Vector3 wallNormal;

    protected override void OnAttach()
    {
        base.OnAttach();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        WallJumpPart part = player.AddComponent<WallJumpPart>();
        part.held = true;

        Destroy(this);
    }

    void Update()
    {
        if (!held) return;

        if (playerMovement == null) playerMovement = GetComponent<PlayerMovement>();

        if (rb == null) rb = GetComponent<Rigidbody>();

        if (Input.GetKeyDown(KeyCode.Space) && !playerMovement.grounded && touchingWall)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x,0f,rb.linearVelocity.z);

            rb.AddForce(wallNormal * 8f + Vector3.up * 8f, ForceMode.Impulse);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        touchingWall = false;

        foreach (ContactPoint contact in collision.contacts)
        {
            if (Mathf.Abs(contact.normal.y) < 0.3f)
            {
                touchingWall = true;
                wallNormal = contact.normal;
                break;
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        touchingWall = false;
    }
}

