using System.Collections;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class dragonEnemy : Enemy
{
    public GameObject Player;
    public Transform spawnPoint;
    public GameObject fireballPrefab;
    Animator animator;
    bool attackActive = false;
    
    bool playerTakenDmg = false; //used for claw attack to make sure player only takes damage once per attack

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    /*void flyattack()
    {
        if (walltriger.dragerErActiv)
        {
            if (walltriger.dragerErActiv)
            {
                if (walltriger.dragerErActiv)
                {
                    if (breathAttackActive == false)
                {
                    transform.position = Vector3.MoveTowards(
                    transform.position,
                    stopSted.position,
                    speedOfDragon * Time.deltaTime * 2
                );
                }
                if (Vector3.Distance(transform.position, stopSted.position) < 0.01f)
                {
                    breathAttackActive = true;
                    transform.position += new Vector3(0, 7, 0);
                }
                if (breathAttackActive == true)
                {
                    dire = -1;
                    transform.Translate(Vector3.right * speedOfDragon * dire * Time.deltaTime);

                    if (oneTime == true)
                    {
                        Vector3 offset = new Vector3(-3, -4, 0);
                        oneTime = false;
                        Instantiate(zombiePrefab, spawnPoint.position + offset, Quaternion.Euler(0, 0, -22), transform);
                    }
                    }
                }
            }
        }
    }*/
    void fireballAttack()
    {
        Debug.Log("catchauw");
        if (attackActive == true)
        {
            animator.SetTrigger("ShootFireAttack");
            StartCoroutine(FireballDelay());
        }
    }
    void ClawAttack()
    {
        Debug.LogWarning("ClawAttack");
        animator.SetTrigger("ClawAttack");

        StartCoroutine(AttackDelay(2));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(playerTakenDmg) return; 

        if (attackActive == true && collision.gameObject == Player) 
        { 
            Player.GetComponent<PlayerManager>().TakeDamage(1);
            playerTakenDmg = true;
            Debug.LogWarning("Player hit by dragon claw attack");
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (walltriger.dragerErActiv && !attackActive) 
        {
            int chosenattack = Random.Range(0, 1);
            attackActive = true;

            if(Player.transform.position.x - transform.position.x <= 45f && Player.transform.position.y - transform.position.y < 0)
            {
                ClawAttack();
                return;
            }

            switch(chosenattack)
            {
                case 0:
                    fireballAttack();
                    break;
                case 1:
                    
                    break;
            }
        }

    }
    IEnumerator FireballDelay()
    {
        yield return new WaitForSeconds(0.55f);
        Vector3 targetPosition = Player.transform.position;
        Vector2 direction = targetPosition - spawnPoint.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Debug.Log(targetPosition);
        GameObject theball = Instantiate(fireballPrefab, spawnPoint.position, Quaternion.Euler(0, 0, angle+100));
        Fireball fire = theball.GetComponent<Fireball>();
        fire.target = targetPosition;
        StartCoroutine(AttackDelay(1));
    }
    IEnumerator AttackDelay(int i)
    {
        yield return new WaitForSeconds(i);
        attackActive = false;
        playerTakenDmg = false;
    }
    public override void TakeDamage(int damage)
    {
        Debug.Log("haha");
    }
    public override void Die()
    {
        animator.SetTrigger("die");
    }
}
