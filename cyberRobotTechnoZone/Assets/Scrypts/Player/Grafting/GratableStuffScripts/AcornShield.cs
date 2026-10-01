using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class AcornShield : GraftablePart
{
    public static bool AcornShieldw = false;

    [SerializeField] AudioClip AttachSound; // The sound of the player attaching the shield
    [SerializeField] AchievementScreenManager achievementScreenManager; // Reference to the AchievementScreenManager script on canvas
    Animator playerAnim;
    PlayerManager playerManager;

    private InputActionMap actionMap;
    private InputAction blockAction;
    [SerializeField] float blockDelay = 1;
    [SerializeField] float blockDuration = 0.3f;
    bool canBlock = true;
    bool blocking = false;

    void Awake()
    {
        actionMap = new InputActionMap("Player");

        blockAction = actionMap.AddAction("Block");
        blockAction.AddBinding("<Keyboard>/k");

        actionMap.Enable();
    }

    private void Start()
    {
        AcornShieldw = false;
        achievementScreenManager = FindAnyObjectByType<AchievementScreenManager>();
    }
    protected override void OnAttach()
    {
        AcornShieldw = true;
        playerAnim = GetComponentInParent<Animator>();
        AudioSource.PlayClipAtPoint(AttachSound, transform.position, 10f);
        Debug.Log(achievementScreenManager);
        achievementScreenManager = FindAnyObjectByType<AchievementScreenManager>();
        achievementScreenManager.UnlockNewAbility("AcornShield");

    }

    private void FixedUpdate()
    {
        if (held)
        {
            if (playerManager == null) playerManager = FindAnyObjectByType<PlayerManager>();
            if (blockAction.IsPressed() && canBlock)
            {
                canBlock = false;

                blocking = true;
                playerManager.shieldInvincibilty = true;

                playerAnim.SetTrigger("UseShield");

                StartCoroutine(WaitAndStopBlock());
                StartCoroutine(WaitAndAllowBlock());
            }


        }
    }
    IEnumerator WaitAndStopBlock()
    {
        yield return new WaitForSeconds(blockDuration);
        blocking = false;
        playerManager.shieldInvincibilty = false;
    }
    IEnumerator WaitAndAllowBlock()
    {
        yield return new WaitForSeconds(blockDelay);
        canBlock = true;
        Debug.Log("Can block again");
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (!blocking) return;

        if(held && collision.gameObject.CompareTag("Projectile"))
        {
            Vector3 knockbackDirection;
            knockbackDirection = (collision.transform.position - transform.position).normalized;

            if(collision.GetComponent<Fireball>() != null)
            {
                collision.GetComponent<Fireball>().target = collision.transform.position + knockbackDirection * 2f;
            }

        }

        if (held && collision.gameObject.CompareTag("Enemy"))
        {
            Vector3 knockbackDirection;
            knockbackDirection = (collision.transform.position - transform.position).normalized;

            if (collision.GetComponent<Enemy>() == null)
            {
                Destroy(collision.gameObject);
            }
            else
            {
                collision.GetComponent<Enemy>().TakeKnockBack(20f, knockbackDirection);
            }
        }
    }



}
