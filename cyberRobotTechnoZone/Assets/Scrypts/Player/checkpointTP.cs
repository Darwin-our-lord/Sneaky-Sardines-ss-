using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
public class checkpointTP : MonoBehaviour
{
    [SerializeField] public static List<GameObject> check = new List<GameObject>();
    public static int interger = 0;
    bool engang = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach(GameObject obj in check) {Debug.Log(obj.name); }
        if (collision.CompareTag("checkpoint"))
        {
            if (!check.Contains(collision.gameObject))
            {
                check.Add(collision.gameObject);
                interger++;
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("checkpoint"))
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                // Go to previous checkpoint
                if (interger > 0)
                {
                    engang = true;
                    interger--;
                }
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                // Go to next checkpoint
                if (interger < check.Count - 1)
                {
                    engang = true;
                    interger++;
                }
            }
            if (engang) { transform.position = check[interger].transform.position; engang = false;}
        }
    }
                /*if (Input.GetKey(KeyCode.U))
                {
                    if (check.Count > 0) { transform.position = check[interger - 1].transform.position; interger--; }
                }
                if (Input.GetKey(KeyCode.I))
                {
                    if (check.Count > 0) { transform.position = check[interger + 1].transform.position; interger++; }
                }*/
     
    // Update is called once per frame
    void Update()
    {
        if (interger != interger)
        { transform.position = check[interger].transform.position; }

    }
}
