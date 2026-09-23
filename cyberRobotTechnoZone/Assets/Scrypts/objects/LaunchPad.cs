using UnityEngine;

public class LaunchPad : MonoBehaviour
{
    public GameObject player;
    public float fortniteLaunchPadForce = 0f;
    private PlayerMovement playerMovement;
    private Animator playerAnimator; 
    Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player 1");
        rb = player.GetComponent<Rigidbody2D>();
    }
    void ManFlyver()
    {
        rb.AddForceY(fortniteLaunchPadForce);
        playerMovement.grounded = false;
        playerAnimator.Play("Jump", 0, 0f);
        // Animation.DestroyObject;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ManFlyver();

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
