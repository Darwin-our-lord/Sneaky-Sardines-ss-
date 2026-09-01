using UnityEngine;

public class walltriger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("du burgt være skremt");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
