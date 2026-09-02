using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class PlayerMovement : MonoBehaviour
{
    public bool facingRight = true; //used to determine which way the player is facing

    private Rigidbody2D rb; // the rigidbody

    [SerializeField] Animator animator; // lock flipping during the attack animation yesyes >:3

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

    private AudioSource audioSource; // the audio source for the player

    [SerializeField] List<AudioClip> footstepsGrassSounds; // the footsteps sound when the player is walking on grass
    [SerializeField] float footstepInterval = 0.5f; // the time between each footstep sound 
    private bool isPlayingFootstepSound = false; // a boolean to check if the footstep sound is already playing
    [SerializeField] AudioClip hitGroundSound; // the sound of the player hitting the ground

    [SerializeField] AudioClip hitWater; // the sound of the player hitting water
    [SerializeField] AudioClip swimmingWaterSound; // the sound of the player swimming in water

    private bool inWater = false; // a boolean to check if the player is in water or not

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

    [SerializeField] float verticalVelocityAnimDelay = 0.1f; // how far behind (in seconds) the VerticalVelocity fed to the Animator is
    private readonly Queue<float> verticalVelocityHistory = new Queue<float>(); // sliding window of past VerticalVelocity samples

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>(); // fetchng the rigidbody
        baseGravityScale = rb.gravityScale;

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        // make sure the starting scale matches facingRight so we don't start mirrored by accident
        ApplyFacingDirection();
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

            if (animator != null)
            {
                animator.SetTrigger("Jump");
            }

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

        // while the Whip forward attack animation is playing, we don't want the player to flip mid-swing,
        // so we skip updating facingRight (but movement itself still works normally)
        bool isAttacking = animator != null && animator.GetCurrentAnimatorStateInfo(0).IsName("Whip forward");

        // checks if D is pressed and if so, the player walks right and is set to be facing right
        if (Keyboard.current.dKey.isPressed)
        {
            Vector2 velocity = Vector2.zero;
            rb.linearVelocity = Vector2.SmoothDamp(rb.linearVelocity, new Vector2(speed, rb.linearVelocityY), ref velocity, 0.1f);

            if (!isAttacking)
            {
                facingRight = true;
            }

        }

        // checks if D is pressed and if so, the player walks left and is set to be facing left

        if (Keyboard.current.aKey.isPressed)
        {
            Vector2 velocity = Vector2.zero;
            rb.linearVelocity = Vector2.SmoothDamp(rb.linearVelocity, new Vector2(-speed, rb.linearVelocityY), ref velocity, 0.1f);

            if (!isAttacking)
            {
                facingRight = false;
            }
        }

        // apply the visual flip based on the current facing direction
        ApplyFacingDirection();

        // feed the Animator the values its transitions rely on
        if (animator != null)
        {
            animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocityX));
            animator.SetBool("Grounded", grounded);
            animator.SetFloat("VerticalVelocity", GetDelayedVerticalVelocity());
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

        if ( (Keyboard.current.aKey.isPressed || Keyboard.current.dKey.isPressed))
        {
            if (grounded == true)
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

            if (inWater)
            {
                audioSource.UnPause();
            }
        }
        else
        {
            CancelInvoke(nameof(PlayFootstepsound));
            isPlayingFootstepSound = false;
            if (inWater)
            {
                audioSource.Pause();
            }
        }   
    }

    // keeps a sliding window of the last verticalVelocityAnimDelay seconds of rb.linearVelocityY,
    // and returns the oldest sample in that window - i.e. the velocity from ~verticalVelocityAnimDelay seconds ago
    private float GetDelayedVerticalVelocity()
    {
        verticalVelocityHistory.Enqueue(rb.linearVelocityY);

        int delayFrames = Mathf.Max(1, Mathf.RoundToInt(verticalVelocityAnimDelay / Time.fixedDeltaTime));

        if (verticalVelocityHistory.Count > delayFrames)
        {
            return verticalVelocityHistory.Dequeue();
        }

        // not enough history yet (e.g. right at Start) - just use the current value
        return rb.linearVelocityY;
    }

    // flips the player's scale on the X axis so all animations (authored facing right) face the correct direction
    private void ApplyFacingDirection()
    {
        Vector3 scale = transform.localScale;
        float flippedX = Mathf.Abs(scale.x) * (facingRight ? 1f : -1f);

        if (!Mathf.Approximately(scale.x, flippedX))
        {
            scale.x = flippedX;
            transform.localScale = scale;
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
                AudioSource.PlayClipAtPoint(hitGroundSound,transform.position, 10f);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 4)
        {
            
            AudioSource.PlayClipAtPoint(hitWater,transform.position, 10f);
            inWater = true;
            InvokeRepeating(nameof(PlaySwimmingSound),0,swimmingWaterSound.length);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 4)
        {
            CancelInvoke(nameof(PlaySwimmingSound));
            audioSource.Stop();
            inWater = false;
        }
    }

    void PlayFootstepsound()
    {
       AudioSource.PlayClipAtPoint(footstepsGrassSounds[Random.Range(0, footstepsGrassSounds.Count)],transform.position, 10f);

    }
    void PlaySwimmingSound()
    {
            audioSource.Play();

       
    }
}