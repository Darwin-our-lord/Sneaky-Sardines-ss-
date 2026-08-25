using UnityEngine;

public class spinspin : GraftablePart
{
        float wait = 0.5f;
        float time = 0;
        float sub = -100;
        bool a = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    protected override void OnAttach()
    {

        if (a == true)
        {
            transform.position += new Vector3(0.5f, 0.5f, 0);
            if (transform.position.y > sub)
            {
                sub = transform.position.y;
            } 
           
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
            {
                time += Time.deltaTime;
                if (time > wait)
                {
                    time = 0;
                    if (sub > transform.position.y)
                    {
                    sub = transform.position.y;
                    GetComponent<SpriteRenderer>().color = Color.yellow;
                    transform.GetComponentInParent<Rigidbody2D>().gravityScale=0.1f;
                    }else if (sub <= transform.position.y) 
                { 
                    sub = transform.position.y;
                    GetComponent<SpriteRenderer>().color = Color.white;
                    transform.GetComponentInParent<Rigidbody2D>().gravityScale = 1f;
                }
                }
            }
    }
}
