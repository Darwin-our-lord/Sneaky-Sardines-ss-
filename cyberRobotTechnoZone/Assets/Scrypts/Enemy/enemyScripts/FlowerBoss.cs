using System.Collections;
using UnityEngine;

public class FlowerBoss : Enemy
{
    [Header("Fight")]
    [SerializeField] int stompsToKill = 5;
    [SerializeField] Transform arenaLeft;
    [SerializeField] Transform arenaRight;
    [SerializeField] Transform head;

    [Header("Per phase (last value is used once you run out)")]
    [SerializeField] float[] idleTimes = { 1.0f, 0.7f, 0.4f };
    [SerializeField] float[] windupTimes = { 0.9f, 0.7f, 0.5f };
    [SerializeField] float[] jumpDurations = { 1.5f, 1.3f, 1.1f };
    [SerializeField] float[] restTimes = { 1.6f, 1.2f, 0.9f }; // pause after landing
    [SerializeField] int[] jumpsInARow = { 1, 2, 3 };
    [SerializeField] float comboWindupMultiplier = 0.5f; // windup for jumps after the first

    [Header("Jump")]
    [SerializeField] float arcHeight = 6f;

    [Header("Jump away when hit")]
    [SerializeField] float fleeDistance = 8f;
    [SerializeField] float fleeJumpDuration = 0.7f;

    [Header("Stomp")]
    [SerializeField] float playerBounceVelocity = 18f;
    [SerializeField] float hitInvulnTime = 1.5f;
    [SerializeField] float invulnSidePush = 10f;
    [SerializeField] bool invulnerableHurts = false;
    [SerializeField] float stompMargin = 1f;

    [Header("Body")]
    [SerializeField] Collider2D bodyCollider;

    [Header("Head colors")]
    [SerializeField] SpriteRenderer headSprite;
    [SerializeField] Color closedColor = Color.white; // invulnerable
    [SerializeField] Color openColor = new Color(1f, 0.6f, 0.8f); // can be hit

    [Header("On death")]
    [SerializeField] GameObject[] enableOnDeath;
    [SerializeField] GameObject[] disableOnDeath;
    [SerializeField] float destroyDelay = 3f;

    [Header("Contact damage")]
    [SerializeField] float contactKnockbackX = 10f;
    [SerializeField] float contactKnockbackY = 8f;

    GameObject player;
    Collider2D playerCollider;
    Collider2D[] bodyParts;
    Animator animator;
    float groundY;
    float lastContactHit = -999f;
    bool fightStarted = false;
    bool dead = false;
    bool fleeAfterHit = false;
    int stompsTaken = 0;
    float invulnUntil = -999f;

    public bool CanBeHit => fightStarted && !dead && Time.time >= invulnUntil;

    // no Awake so Enemy.Awake runs
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) playerCollider = player.GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        if (bodyCollider == null) bodyCollider = GetComponent<Collider2D>();
        if (headSprite == null && head != null) headSprite = head.GetComponentInChildren<SpriteRenderer>();
        groundY = transform.position.y;

        bodyParts = GetComponentsInChildren<Collider2D>();
        if (playerCollider != null)
        {
            foreach (Collider2D part in bodyParts)
            {
                if (!part.isTrigger) Physics2D.IgnoreCollision(part, playerCollider, true);
            }
        }
    }

    void Update()
    {
        if (headSprite != null) headSprite.color = CanBeHit ? openColor : closedColor;
    }

    // checks overlap every physics step instead of relying on collision events
    void FixedUpdate()
    {
        if (!fightStarted || dead || playerCollider == null) return;

        foreach (Collider2D part in bodyParts)
        {
            if (part == null || part.isTrigger || !part.enabled) continue;

            ColliderDistance2D d = Physics2D.Distance(part, playerCollider);
            if (d.isValid && d.distance <= 0.05f)
            {
                HandleBodyContact(playerCollider);
                return;
            }
        }
    }

    public void StartFight()
    {
        if (fightStarted || dead) return;
        fightStarted = true;
        player.GetComponentInChildren<Camera>().orthographicSize = 25f;
        StartCoroutine(FightLoop());
    }

    float PhaseValue(float[] arr) => arr[Mathf.Min(stompsTaken, arr.Length - 1)];
    int PhaseValue(int[] arr) => arr[Mathf.Min(stompsTaken, arr.Length - 1)];

    IEnumerator FightLoop()
    {
        while (!dead)
        {
            if (!fleeAfterHit)
            {
                yield return new WaitForSeconds(PhaseValue(idleTimes));
                if (player == null) yield break;

                int jumps = Mathf.Max(1, PhaseValue(jumpsInARow));
                for (int i = 0; i < jumps && !dead && !fleeAfterHit; i++)
                {
                    float windup = PhaseValue(windupTimes) * (i == 0 ? 1f : comboWindupMultiplier);

                    // target locked at windup so player can dodge
                    Vector3 target = new Vector3(ClampToArena(player.transform.position.x), groundY, transform.position.z);

                    SetTrigger("Windup");
                    yield return new WaitForSeconds(windup);
                    if (fleeAfterHit) break;

                    SetTrigger("Jump");
                    yield return JumpTo(target, PhaseValue(jumpDurations));
                    SetTrigger("Land");
                }

                float timer = 0f;
                while (timer < PhaseValue(restTimes) && !fleeAfterHit)
                {
                    timer += Time.deltaTime;
                    yield return null;
                }
            }

            if (fleeAfterHit && !dead)
            {
                fleeAfterHit = false;
                SetTrigger("Jump");
                yield return JumpTo(FleeTarget(), fleeJumpDuration);
                SetTrigger("Land");
            }
        }
    }

    IEnumerator JumpTo(Vector3 target, float duration)
    {
        Vector3 start = transform.position;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.fixedDeltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float height = Mathf.Sin(t * Mathf.PI) * arcHeight;
            transform.position = Vector3.Lerp(start, target, t) + Vector3.up * height;
            yield return new WaitForFixedUpdate();
        }
        transform.position = target;
    }

    Vector3 FleeTarget()
    {
        float dir = Mathf.Sign(transform.position.x - player.transform.position.x);
        float x = ClampToArena(transform.position.x + dir * fleeDistance);

        // cornered, jump the other way
        if (Mathf.Abs(x - transform.position.x) < fleeDistance * 0.5f)
        {
            x = ClampToArena(transform.position.x - dir * fleeDistance);
        }

        return new Vector3(x, groundY, transform.position.z);
    }

    float ClampToArena(float x)
    {
        if (arenaLeft == null || arenaRight == null) return x;
        return Mathf.Clamp(x, arenaLeft.position.x, arenaRight.position.x);
    }

    public void TryStomp(Rigidbody2D playerRb)
    {
        if (dead || !fightStarted || playerRb == null) return;
        if (playerRb.linearVelocityY > 0.1f) return; // must be falling

        if (!CanBeHit)
        {
            // knock off so you don't keep bouncing on it
            float side = Mathf.Sign(playerRb.position.x - transform.position.x);
            if (side == 0f) side = 1f;
            BouncePlayer(playerRb, side * invulnSidePush);
            if (invulnerableHurts) playerRb.GetComponent<PlayerManager>()?.TakeDamage(1);
            return;
        }

        BouncePlayer(playerRb, playerRb.linearVelocityX);
        stompsTaken++;
        invulnUntil = Time.time + hitInvulnTime;
        fleeAfterHit = true;
        SpawnDamagePrefab(head != null ? head.position : transform.position);
        SetTrigger("Hurt");

        if (stompsTaken >= stompsToKill)
        {
            Die();
        }
    }

    void BouncePlayer(Rigidbody2D playerRb, float velocityX) => BouncePlayer(playerRb, velocityX, playerBounceVelocity);

    void BouncePlayer(Rigidbody2D playerRb, float velocityX, float velocityY)
    {
        playerRb.linearVelocity = new Vector2(velocityX, velocityY);
        PlayerMovement movement = playerRb.GetComponent<PlayerMovement>();
        if (movement != null)
        {
            movement.grounded = false;
            movement.enabled = false;
            StartCoroutine(ReenableMovement(movement));
        }
    }

    IEnumerator ReenableMovement(PlayerMovement movement)
    {
        yield return new WaitForSeconds(0.2f);
        if (movement != null) movement.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision) => HandleBodyContact(collision.collider);
    private void OnCollisionStay2D(Collision2D collision) => HandleBodyContact(collision.collider);
    public void HandleBodyContact(Collider2D other)
    {
        if (dead || !fightStarted || !other.CompareTag("Player")) return;

        // landing on top counts as a stomp, not a hit
        if (PlayerOnTop(other))
        {
            TryStomp(other.attachedRigidbody);
            return;
        }
        if (Time.time < invulnUntil - hitInvulnTime + 0.3f) return; // just got stomped
        if (Time.time - lastContactHit < 0.5f) return;
        lastContactHit = Time.time;

        other.GetComponent<PlayerManager>()?.TakeDamage(1);

        // push the player away so the hit is felt and they don't stand inside it
        Rigidbody2D rb = other.attachedRigidbody;
        if (rb != null)
        {
            float side = Mathf.Sign(rb.position.x - transform.position.x);
            if (side == 0f) side = 1f;
            BouncePlayer(rb, side * contactKnockbackX, contactKnockbackY);
        }
    }

    bool PlayerOnTop(Collider2D playerCollider)
    {
        if (bodyCollider == null) return false;
        return playerCollider.bounds.min.y >= bodyCollider.bounds.max.y - stompMargin;
    }

    // only stomps hurt it
    public override void TakeDamage(int damage) { }

    public override void Die()
    {
        if (dead) return;
        dead = true;

        player.GetComponentInChildren<Camera>().orthographicSize = 15.23f; //reset camera size

        StopAllCoroutines();
        if (player != null) player.GetComponent<PlayerMovement>().enabled = true;
        if (headSprite != null) headSprite.color = closedColor;
        SetTrigger("die");

        foreach (GameObject obj in enableOnDeath) if (obj != null) obj.SetActive(true);
        foreach (GameObject obj in disableOnDeath) if (obj != null) obj.SetActive(false);

        Destroy(gameObject, destroyDelay);
    }

    void SetTrigger(string name)
    {
        if (animator != null && animator.runtimeAnimatorController != null)
        {
            animator.SetTrigger(name);
        }
    }
}