using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class PlayerMovement : MonoBehaviour
{
    public bool facingRight = true; //used to determine which way the player is facing

    private Rigidbody2D rb; // the rigidbody

    [SerializeField] float speed; // the speed of wich the player moves
    [SerializeField] float jumpHeight; // the height of wich the player jumps
    [SerializeField] float maxJumpAngle = 45f; // the maximum angle of the slope wich the player can jump on

    private int groundContactCount;
    private bool grounded => groundContactCount > 0;

    [SerializeField] float jumpCooldown = 1f; // the time between jumps - this fixes issues with double jumping on a single frame
    private float currentCooldownTime; // the timer itself

    [SerializeField] float cyoteTime; // the time after the player leaves the ground, but still is able to jump
    private float currentCyoteTime; // the timer itself
    public bool canJump { get; private set; }

    [SerializeField] List<AudioClip> footstepsGrassSounds; // the footsteps sound when the player is walking on grass
    [SerializeField] float footstepInterval = 0.5f; // the time between each footstep sound 
    private bool isPlayingFootstepSound = false; // a boolean to check if the footstep sound is already playing
    [SerializeField] AudioClip hitGroundSound; // the sound of the player hitting the ground


    [SerializeField] float minAirTimeForLandSound = 0.3f;
    [SerializeField] float minFallSpeedForLandSound = 5f;
    private float airTimeTimer = 0f;
    private float fallSpeedAtLastCheck = 0f;

    [SerializeField] float groundDragRate = 3f;


    [SerializeField] float airDragRate = 0.2f;

    [SerializeField] float dragTransitionSharpness = 4f;
    private float currentDragRate = 0f;

    private float baseGravityScale;
    [SerializeField] float fallGravityMultiplier = 2f;
    [SerializeField] float lowJumpGravityMultiplier = 4f;
    [SerializeField] float jumpCutMultiplier = 0.5f;
    private bool wasJumpKeyHeld;

    [SerializeField] float gravityTransitionSharpness = 6f;
    private float currentGravityMultiplier = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // fetchng the rigidbody
        baseGravityScale = rb.gravityScale;
    }


    void FixedUpdate()
    {
        // making the cooldown timer go down, and stops when it's under 0 so it doesnt run forever
        if (currentCooldownTime >= 0)
        {
            currentCooldownTime -= 0.1f;

        }

        if (grounded == false)
        {
            // making the cyote timer go down, and when it reaches zero, the player can no longer jump in the air
            if (currentCyoteTime >= 0)
            {
                currentCyoteTime -= 0.1f;

            }
            else
            {
                canJump = false;
            }

            airTimeTimer += Time.fixedDeltaTime;
            if (rb.linearVelocityY < 0f)
            {
                fallSpeedAtLastCheck = rb.linearVelocityY;
            }
        }


        // checks if W or Space is pressend while the player is on the ground and the cooldown has passed - if so, the player jumps 
        bool jumpKeyHeld = Keyboard.current.wKey.isPressed || Keyboard.current.spaceKey.isPressed;
        if (jumpKeyHeld && canJump == true && currentCooldownTime <= 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpHeight);

            currentCooldownTime = jumpCooldown;

            groundContactCount = 0;
            canJump = false;

        }


        if (wasJumpKeyHeld && !jumpKeyHeld && rb.linearVelocityY > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, rb.linearVelocityY * jumpCutMultiplier);
        }
        wasJumpKeyHeld = jumpKeyHeld;

        float stateMultiplier;
        if (rb.linearVelocityY < 0f)
        {
            stateMultiplier = fallGravityMultiplier;
        }
        else if (rb.linearVelocityY > 0f && !jumpKeyHeld)
        {
            stateMultiplier = lowJumpGravityMultiplier;
        }
        else
        {
            stateMultiplier = 1f;
        }
        currentGravityMultiplier = Mathf.Lerp(currentGravityMultiplier, stateMultiplier, 1f - Mathf.Exp(-gravityTransitionSharpness * Time.fixedDeltaTime));
        rb.gravityScale = baseGravityScale * currentGravityMultiplier;

        // checks if D is pressed and if so, the player walks right and is set to be facing right
        if (Keyboard.current.dKey.isPressed)
        {
            Vector2 velocity = Vector2.zero;
            rb.linearVelocity = Vector2.SmoothDamp(rb.linearVelocity, new Vector2(speed, rb.linearVelocityY), ref velocity, 0.1f);
            facingRight = true;


        }

        // checks if D is pressed and if so, the player walks left and is set to be facing left

        if (Keyboard.current.aKey.isPressed)
        {
            Vector2 velocity = Vector2.zero;
            rb.linearVelocity = Vector2.SmoothDamp(rb.linearVelocity, new Vector2(-speed, rb.linearVelocityY), ref velocity, 0.1f);
            facingRight = false;
        }

        if (!Keyboard.current.aKey.isPressed && !Keyboard.current.dKey.isPressed && Mathf.FloatToHalf(rb.linearVelocityX) != 0)
        {
            // if (grounded == true)
            // {
            //     rb.linearVelocityX *= 0.8f;
            // }
            // else
            // {
            //     rb.linearVelocityX *= 0.999f;
            // }
            float targetDragRate = grounded ? groundDragRate : airDragRate;
            currentDragRate = Mathf.Lerp(currentDragRate, targetDragRate, 1f - Mathf.Exp(-dragTransitionSharpness * Time.fixedDeltaTime));
            rb.linearVelocityX = Mathf.Lerp(rb.linearVelocityX, 0f, 1f - Mathf.Exp(-currentDragRate * Time.fixedDeltaTime));
        }

        if (grounded == true && (Keyboard.current.aKey.isPressed || Keyboard.current.dKey.isPressed))
        {
            if (!isPlayingFootstepSound)
            {
                InvokeRepeating(nameof(PlayFootstepsound), 0f, footstepInterval);
                isPlayingFootstepSound = true;
            }
        }
        else
        {
            CancelInvoke(nameof(PlayFootstepsound));
            isPlayingFootstepSound = false;

        }
    }
    // when the player �s colliding with something with the layer "Ground" and the normal of the contactpoint is pointing upwards (if the player is on top of the collider) -
    // grounded is set to true
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3 && Vector2.Angle(collision.GetContact(0).normal, Vector2.up) <= maxJumpAngle && rb.linearVelocityY <= 0)
        {

            canJump = true;

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            groundContactCount++;
        }
        if (groundContactCount == 1)
        {
            // AudioSource.PlayClipAtPoint(hitGroundSound, transform.position, 10f);
            if (airTimeTimer >= minAirTimeForLandSound && Mathf.Abs(fallSpeedAtLastCheck) >= minFallSpeedForLandSound)
            {
                AudioSource.PlayClipAtPoint(hitGroundSound, transform.position, 600f);
            }
            airTimeTimer = 0f;
            fallSpeedAtLastCheck = 0f;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            groundContactCount = Mathf.Max(0, groundContactCount - 1);
            if (!grounded)
            {
                currentCyoteTime = cyoteTime;
            }
        }
    }


    void PlayFootstepsound()
    {
        AudioSource.PlayClipAtPoint(footstepsGrassSounds[Random.Range(0, footstepsGrassSounds.Count)], transform.position, 40f);

    }

}