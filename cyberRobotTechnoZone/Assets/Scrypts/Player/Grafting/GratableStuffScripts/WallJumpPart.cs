using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using static Unity.U2D.Physics.PhysicsShape;

public class WallJumpPart : GraftablePart
{
    PlayerMovement playerMovement;
    Rigidbody2D rb;

    public bool touchingWall;
    Vector3 wallNormal;

    private InputActionMap actionMap;
    private InputAction jumpAction;
    private bool wasJumpKeyHeld;

    private float jumpForce = 20;

    void SetInputs()
    {
        actionMap = new InputActionMap("Player");

        jumpAction = actionMap.AddAction("Jump");
        jumpAction.AddBinding("<Keyboard>/space");
        jumpAction.AddBinding("<Keyboard>/w");
        jumpAction.AddBinding("<Keyboard>/upArrow");

        actionMap.Enable();
    }

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

        if (actionMap == null) SetInputs();

        if (playerMovement == null) playerMovement = GetComponent<PlayerMovement>();

        if (rb == null) rb = GetComponent<Rigidbody2D>();

        bool jumpKeyHeld = jumpAction.IsPressed();
        bool jumpKeyPressedThisFrame = jumpKeyHeld && !wasJumpKeyHeld;
        if (jumpKeyPressedThisFrame && !playerMovement.grounded && touchingWall)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x,0f);
            rb.AddForce(wallNormal * jumpForce + Vector3.up * jumpForce, ForceMode2D.Impulse);
        }
        wasJumpKeyHeld = jumpKeyHeld;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        //touchingWall = false;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (Mathf.Abs(contact.normal.y) < 0.3f)
            {
                touchingWall = true;
                wallNormal = contact.normal;
                rb.AddForce(new Vector3(0,0.5f,0),ForceMode2D.Force);
                break;
            }
        }
    }


    void OnCollisionExit2D(Collision2D collision)
    {
        touchingWall = false;
    }
}

