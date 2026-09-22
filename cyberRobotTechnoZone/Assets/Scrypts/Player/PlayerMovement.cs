using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class PlayerMovement : MonoBehaviour
{
    public bool facingRight = true;

    private Rigidbody2D rb;
    private Collider2D col; // for the ground raycasts n the friction fix below

    [SerializeField] Animator animator; // lock flipping during the attack animation yesyes >:3

    [SerializeField] float maxMoveSpeed = 10f;
    [SerializeField] float groundAcceleration = 120f;
    [SerializeField] float groundDeceleration = 140f;
    [SerializeField] float airAcceleration = 90f;
    [SerializeField] float airDeceleration = 60f;

    [SerializeField] float jumpHeight; // the height of wich the player jumps (ok its actually a velocity but renaming it means re-wiring the inspector so. jumpHeight it stays)
    [SerializeField] float maxJumpAngle = 45f; // the maximum angle of the slope wich the player can jump on

    [SerializeField] LayerMask groundLayer; // set this to ur ground layer or nothing's ever grounded lol
    [SerializeField] float groundCheckDistance = 0.1f; // how far below the feet we check for ground
    public bool grounded;
    private Vector2 groundNormal = Vector2.up;

    [SerializeField] float cyoteTime; // the time after the player leaves the ground, but still is able to jump
    private float currentCyoteTime; // the timer itself
    public bool canJump { get; private set; }

    [SerializeField] float jumpBufferTime = 0.1f; // press jump juuust before landing and it still counts
    private float jumpBufferTimer = 0f;

    private AudioSource audioSource; // the audio source for the player

    private InputActionMap actionMap;
    private InputAction moveAction;
    private InputAction jumpAction;

    void Awake()
    {
        actionMap = new InputActionMap("Player");

        moveAction = actionMap.AddAction("Move");
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Up", "<Keyboard>/upArrow")
            .With("Down", "<Keyboard>/s")
            .With("Down", "<Keyboard>/downArrow")
            .With("Left", "<Keyboard>/a")
            .With("Left", "<Keyboard>/leftArrow")
            .With("Right", "<Keyboard>/d")
            .With("Right", "<Keyboard>/rightArrow");

        jumpAction = actionMap.AddAction("Jump");
        jumpAction.AddBinding("<Keyboard>/space");
        jumpAction.AddBinding("<Keyboard>/w");
        jumpAction.AddBinding("<Keyboard>/upArrow");
    }

    void OnEnable()
    {
        actionMap?.Enable();
    }

    void OnDisable()
    {
        actionMap?.Disable();
    }

    void OnDestroy()
    {
        actionMap?.Dispose();
    }

    [SerializeField] List<AudioClip> footstepsGrassSounds; // the footsteps sound when the player is walking on grass
    [SerializeField] float footstepInterval = 0.5f; // the time between each footstep sound 
    private bool isPlayingFootstepSound = false; // a boolean to check if the footstep sound is already playing
    [SerializeField] AudioClip hitGroundSound; // the sound of the player hitting the ground

    [SerializeField] AudioClip hitWater; // the sound of the player hitting water
    [SerializeField] AudioClip swimmingWaterSound; // the sound of the player swimming in water

    private bool inWater = false; // a boolean to check if the player is in water or not

    [SerializeField] float minAirTimeForLandSound = 0.3f; // gotta be falling this long for the thud sound
    [SerializeField] float minFallSpeedForLandSound = 5f; // and falling this fast, else no thud
    private float airTimeTimer = 0f;
    private float fallSpeedAtLastCheck = 0f;

    private float baseGravityScale;
    [SerializeField] float fallGravityMultiplier = 2f; // falling is snappier than rising
    [SerializeField] float lowJumpGravityMultiplier = 4f; // tap jump = short hop
    [SerializeField] float jumpCutMultiplier = 0.5f; // instant lil knock-down when you let go early
    private bool wasJumpKeyHeld;

    [SerializeField] float gravityTransitionSharpness = 6f; // how quick gravity eases between the above, so it's not jerky
    private float currentGravityMultiplier = 1f;

    [SerializeField] float verticalVelocityAnimDelay = 0.1f; // how far behind (in seconds) the VerticalVelocity fed to the Animator is
    private readonly Queue<float> verticalVelocityHistory = new Queue<float>(); // sliding window of past VerticalVelocity samples

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>(); // fetchng the rigidbody
        col = GetComponent<Collider2D>();
        baseGravityScale = rb.gravityScale;

        if (col != null)
        {
            PhysicsMaterial2D noFriction = new PhysicsMaterial2D("PlayerNoFriction") { friction = 0f, bounciness = 0f };
            col.sharedMaterial = noFriction;
        }

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        ApplyFacingDirection();
    }


    void FixedUpdate()
    {
        bool justJumped = false; // used further down so jumping doesnt get instantly cancelled by ground movement, see below

        bool wasGrounded = grounded;
        // ignore ground hits while still going up, else clipping a ledge corner mid-jump gets you stuck
        grounded = CheckGrounded(out Vector2 hitNormal) && rb.linearVelocityY <= 0.1f;
        if (grounded)
        {
            groundNormal = hitNormal;
        }

        bool justLanded = grounded && !wasGrounded;

        if (grounded)
        {
            currentCyoteTime = cyoteTime;
            canJump = true;
        }
        else
        {
            // making the cyote timer go down, and when it reaches zero, the player can no longer jump in the air
            if (currentCyoteTime > 0f)
            {
                currentCyoteTime -= Time.fixedDeltaTime;
                canJump = true;
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

        if (justLanded)
        {
            if (airTimeTimer >= minAirTimeForLandSound && Mathf.Abs(fallSpeedAtLastCheck) >= minFallSpeedForLandSound)
            {
                AudioSource.PlayClipAtPoint(hitGroundSound, transform.position, 10f);
            }
            airTimeTimer = 0f;
            fallSpeedAtLastCheck = 0f;
        }

        bool jumpKeyHeld = jumpAction.IsPressed();
        bool jumpKeyPressedThisFrame = jumpKeyHeld && !wasJumpKeyHeld;

        if (jumpKeyPressedThisFrame)
        {
            jumpBufferTimer = jumpBufferTime;
        }
        else if (jumpBufferTimer > 0f)
        {
            jumpBufferTimer -= Time.fixedDeltaTime;
        }

        if (jumpBufferTimer > 0f && canJump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpHeight);

            canJump = false;
            currentCyoteTime = 0f;
            jumpBufferTimer = 0f;
            justJumped = true;

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
        if (!grounded)
        {
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
        }
        else
        {
            stateMultiplier = 1f;
        }
        currentGravityMultiplier = Mathf.Lerp(currentGravityMultiplier, stateMultiplier, 1f - Mathf.Exp(-gravityTransitionSharpness * Time.fixedDeltaTime));
        rb.gravityScale = baseGravityScale * currentGravityMultiplier;
        bool isAttacking = animator != null && animator.GetCurrentAnimatorStateInfo(0).IsName("Whip forward");

        Vector2 slopeRight = grounded ? new Vector2(groundNormal.y, -groundNormal.x) : Vector2.right;

        float inputDir = moveAction.ReadValue<Vector2>().x; // -1 left, 1 right, 0 chillin

        if (!isAttacking && inputDir != 0f)
        {
            facingRight = inputDir > 0f;
        }

        bool treatAsGrounded = grounded && !justJumped;

        float targetSpeed = inputDir * maxMoveSpeed;
        float accelRate;
        if (treatAsGrounded)
        {
            accelRate = inputDir != 0f ? groundAcceleration : groundDeceleration;
        }
        else
        {
            accelRate = inputDir != 0f ? airAcceleration : airDeceleration;
        }

        if (treatAsGrounded)
        {
            // walk along the slope instead of straight sideways, so hills dont feel weird
            float currentTangentSpeed = Vector2.Dot(rb.linearVelocity, slopeRight);
            float newTangentSpeed = Mathf.MoveTowards(currentTangentSpeed, targetSpeed, accelRate * Time.fixedDeltaTime);
            rb.linearVelocity = slopeRight * newTangentSpeed;
        }
        else
        {
            float newVelocityX = Mathf.MoveTowards(rb.linearVelocityX, targetSpeed, accelRate * Time.fixedDeltaTime);
            rb.linearVelocity = new Vector2(newVelocityX, rb.linearVelocityY);
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

        if (inputDir != 0f)
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

    // 3 lil rays under the feet instead of collision events, closest hit wins for the slope normal
    private bool CheckGrounded(out Vector2 normal)
    {
        normal = Vector2.up;
        if (col == null) return false;

        Bounds bounds = col.bounds;
        float inset = 0.05f;
        Vector2[] origins = new Vector2[]
        {
            new Vector2(bounds.min.x + inset, bounds.min.y + 0.02f),
            new Vector2(bounds.center.x, bounds.min.y + 0.02f),
            new Vector2(bounds.max.x - inset, bounds.min.y + 0.02f)
        };

        bool foundGround = false;
        float closestDistance = float.MaxValue;

        foreach (Vector2 origin in origins)
        {
            RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, groundCheckDistance, groundLayer);
            if (hit.collider != null && Vector2.Angle(hit.normal, Vector2.up) <= maxJumpAngle)
            {
                foundGround = true;
                if (hit.distance < closestDistance)
                {
                    closestDistance = hit.distance;
                    normal = hit.normal;
                }
            }
        }

        return foundGround;
    }

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 4)
        {

            AudioSource.PlayClipAtPoint(hitWater, transform.position, 10f);
            inWater = true;
            InvokeRepeating(nameof(PlaySwimmingSound), 0, swimmingWaterSound.length);
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
        AudioSource.PlayClipAtPoint(footstepsGrassSounds[Random.Range(0, footstepsGrassSounds.Count)], transform.position, 6f);

    }
    void PlaySwimmingSound()
    {
        audioSource.Play();


    }
}