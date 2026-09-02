using System.Collections;
using UnityEngine;

public class dragonEnemy : Enemy
{
    public GameObject Player;
    public Transform spawnPoint;
    public GameObject fireballPrefab;
    bool attackActive = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
            Vector3 targetPosition = Player.transform.position;
            Debug.Log(targetPosition);
            GameObject theball = Instantiate(fireballPrefab, spawnPoint.position, Quaternion.Euler(0, 0, 0));
            Fireball fire = theball.GetComponent<Fireball>();
            fire.target = targetPosition;
            StartCoroutine(AttackDelay());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (walltriger.dragerErActiv && !attackActive) 
        {
            int chosenattack = Random.Range(0, 1);
            attackActive = true;
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
    IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(2);
        attackActive = false;
    }
}
