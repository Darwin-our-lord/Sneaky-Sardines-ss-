using System.Collections;
using UnityEngine;

public class jumperEnemy : Enemy
{
    [SerializeField] float speed = 0.01f;
    [SerializeField] float jumpRange = 6f;
    [SerializeField] float jumpMinRange = 6f;
    [SerializeField] float jumpDuration = 0.6f;
    [SerializeField] float arcHeight = 2f;
    [SerializeField] float jumpCooldown = 3f;

    GameObject player;
    bool isJumping = false;
    float jumpTimer = 0f;
    Coroutine currentJumpCoroutine;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()
    {
        if (player == null) return;

        jumpTimer -= Time.fixedDeltaTime;

        float dist = Vector3.Distance(transform.position, player.transform.position);

        if (!isJumping && jumpTimer <= 0f && dist <= jumpRange && dist >= jumpMinRange)
        {
            currentJumpCoroutine = StartCoroutine(JumpAtPlayer());
            jumpTimer = jumpCooldown;
        }

        if (!isJumping && dist < 15f)
        {
            if (!isJumping && dist < jumpMinRange)
            {
                Vector3 dir = (transform.position - player.transform.position).normalized;
                transform.position += dir * speed;
            }
            if (!isJumping && dist > jumpRange)
            {
                Vector3 dir = (player.transform.position - transform.position).normalized;
                transform.position += dir * speed;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.GetComponent<PlayerManager>().TakeDamage(1);
        }
    }

    IEnumerator JumpAtPlayer()
    {
        isJumping = true;
        Vector3 start = transform.position;
        Vector3 target = player != null ? player.transform.position + new Vector3(0, 0.2f, 0) : start;
        float elapsed = 0f; //time since jump started

        while (elapsed < jumpDuration)
        {
            elapsed += Time.fixedDeltaTime;
            float t = Mathf.Clamp01(elapsed / jumpDuration);
            float height = Mathf.Sin(t * Mathf.PI) * arcHeight;//calculate the height of the jump using a sine wave for a smooth arc
            Vector3 pos = Vector3.Lerp(start, target, t) + Vector3.up * height;//find where the player should be along the jump path and add the height to it
            transform.position = pos;
            yield return new WaitForFixedUpdate();//wait until the next physics update to continue the jump
        }

        transform.position = target;
        isJumping = false;
        currentJumpCoroutine = null;
    }

    // cancel jump when knocked back
    protected override void OnKnockback()
    {
        if (currentJumpCoroutine != null)
        {
            StopCoroutine(currentJumpCoroutine);
            currentJumpCoroutine = null;
        }
        isJumping = false;
    }
}
