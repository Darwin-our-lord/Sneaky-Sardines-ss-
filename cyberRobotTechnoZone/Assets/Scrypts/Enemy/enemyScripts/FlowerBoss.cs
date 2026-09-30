using System.Collections;
using UnityEngine;

public class FlowerBoss : Enemy
{
    [Header("Fight")]
    [SerializeField] int stompsToKill = 3;
    [SerializeField] Transform arenaLeft;
    [SerializeField] Transform arenaRight;
    [SerializeField] Transform head;

    [Header("Per phase")]
    [SerializeField] float[] idleTimes = { 1.2f, 0.8f, 0.5f };
    [SerializeField] float[] windupTimes = { 0.9f, 0.7f, 0.5f };
    [SerializeField] float[] jumpDurations = { 1.1f, 0.9f, 0.7f };
    [SerializeField] float[] dazeTimes = { 2.5f, 1.8f, 1.3f };

    [Header("Jump")]
    [SerializeField] float arcHeight = 6f;

    [Header("Stomp")]
    [SerializeField] float playerBounceVelocity = 18f;
    [SerializeField] float stompInvulnTime = 0.5f;

    [Header("On death")]
    [SerializeField] GameObject[] enableOnDeath;
    [SerializeField] GameObject[] disableOnDeath;
    [SerializeField] float destroyDelay = 3f;

    GameObject player;
    Animator animator;
    float groundY;
    bool fightStarted = false;
    bool dead = false;
    bool bloomOpen = false;
    int stompsTaken = 0;
    float lastStompTime = -999f;

    public bool BloomOpen => bloomOpen;

    // no Awake so Enemy.Awake runs
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        groundY = transform.position.y;
    }

    public void StartFight()
    {
        if (fightStarted || dead) return;
        fightStarted = true;
        StartCoroutine(FightLoop());
    }

    int Phase => Mathf.Clamp(stompsTaken, 0, 2);
    float PhaseValue(float[] arr) => arr[Mathf.Min(Phase, arr.Length - 1)];

    IEnumerator FightLoop()
    {
        while (!dead)
        {
            yield return new WaitForSeconds(PhaseValue(idleTimes));
            if (player == null) yield break;

            // target locked at windup so player can dodge
            float targetX = player.transform.position.x;
            if (arenaLeft != null && arenaRight != null)
            {
                targetX = Mathf.Clamp(targetX, arenaLeft.position.x, arenaRight.position.x);
            }
            Vector3 target = new Vector3(targetX, groundY, transform.position.z);

            SetTrigger("Windup");
            yield return new WaitForSeconds(PhaseValue(windupTimes));

            SetTrigger("Jump");
            yield return JumpTo(target, PhaseValue(jumpDurations));
            SetTrigger("Land");

            // stomp window
            bloomOpen = true;
            SetTrigger("Bloom");
            float timer = 0f;
            while (timer < PhaseValue(dazeTimes) && bloomOpen)
            {
                timer += Time.deltaTime;
                yield return null;
            }
            if (bloomOpen)
            {
                bloomOpen = false;
                SetTrigger("Close");
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

    public void TryStomp(Rigidbody2D playerRb)
    {
        if (dead || !fightStarted || playerRb == null) return;
        if (playerRb.linearVelocityY > 0.1f) return; // must be falling
        if (Time.time - lastStompTime < stompInvulnTime) return;
        lastStompTime = Time.time;

        BouncePlayer(playerRb);

        if (!bloomOpen)
        {
            playerRb.GetComponent<PlayerManager>()?.TakeDamage(1);
            return;
        }

        stompsTaken++;
        bloomOpen = false;
        SpawnDamagePrefab();
        SetTrigger("Hurt");

        if (stompsTaken >= stompsToKill)
        {
            Die();
        }
    }

    void BouncePlayer(Rigidbody2D playerRb)
    {
        playerRb.linearVelocity = new Vector2(playerRb.linearVelocityX, playerBounceVelocity);
        PlayerMovement movement = playerRb.GetComponent<PlayerMovement>();
        if (movement != null) movement.grounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (dead || !collision.collider.CompareTag("Player")) return;
        if (head != null && collision.transform.position.y > head.position.y) return; // ignore head landings
        collision.collider.GetComponent<PlayerManager>()?.TakeDamage(1);
    }

    // only stomps hurt it
    public override void TakeDamage(int damage) { }

    public override void Die()
    {
        if (dead) return;
        dead = true;
        bloomOpen = false;
        StopAllCoroutines();
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