using UnityEngine;
using UnityEngine.InputSystem;

public class spinspin : GraftablePart
{
    private float lastYPosition = -100f;
    private bool isGlideSuppressed = false;

    private Rigidbody2D parentRb;
    private PlayerMovement player;

    protected override void OnAttach()
    {
        parentRb = GetComponentInParent<Rigidbody2D>();
        player = GetComponentInParent<PlayerMovement>();

        transform.localPosition += new Vector3(0.5f, 0.5f, 0f);

        if (transform.position.y > lastYPosition)
        {
            lastYPosition = transform.position.y;
        }
    }

    protected override void OnDetach()
    {
        if (parentRb != null)
        {
            parentRb.gravityScale = 1f;
        }

        parentRb = null;
        player = null;
    }

    private void FixedUpdate()
    {
        if (!held || player == null || parentRb == null) return;

        isGlideSuppressed = Keyboard.current != null && Keyboard.current.sKey.isPressed;

        if (player.canJump)
        {
            lastYPosition = transform.position.y;
            parentRb.gravityScale = 1f;
        }
        else
        {
            if (transform.position.y < lastYPosition && !isGlideSuppressed)
            {
                parentRb.gravityScale = 0.2f;
            }
            else
            {
                parentRb.gravityScale = 1f;
            }

            lastYPosition = transform.position.y;
        }
    }
}