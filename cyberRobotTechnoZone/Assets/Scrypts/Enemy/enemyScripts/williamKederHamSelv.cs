using UnityEngine;

public class williamKederHamSelv : MonoBehaviour
{
    public Transform stopSted;
    public GameObject Player;
    public GameObject zombiePrefab;
    public Transform spawnPoint;
    public GameObject fireballPrefab;
    float speedOfDragon = 2f;
    float dire = 1;
    bool breathAttackActive = false;
    bool erVedNi = false;
    bool oneTime = true;
    bool fireballAtttack = true;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    void flyattack()
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
    }
    void fireballAttack()
    {
        Debug.Log("catchauw");
        if (fireballAtttack == true)
        {
            Vector3 targetPosition = Player.transform.position;

            GameObject theball = Instantiate(fireballPrefab, spawnPoint.position, Quaternion.Euler(0, 0, 0));
            Fireball fire = theball.GetComponent<Fireball>();
            fire.target = targetPosition;
            fireballAtttack = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (walltriger.dragerErActiv) 
        {
            fireballAttack();
            //flyattack();
        }

    }
}
