using UnityEngine;

public class walltriger : MonoBehaviour
{
    public static bool dragerErActiv = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dragerErActiv = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            dragerErActiv = true;
        }
        Debug.Log("du burgt være skremt");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
