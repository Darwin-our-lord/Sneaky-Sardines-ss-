using UnityEngine;

public class chechPoint : MonoBehaviour
{
    public GameObject player;

    public static bool HasSpispin = false;
    public static bool HasVineWhip = false;
    public static bool AcornShild = false;
    public static bool walljump = false;
    [SerializeField] GameObject checkpointSpinspin;
    [SerializeField] GameObject checkpointVineWhip;
    [SerializeField] GameObject checkpointAcornShield;
    [SerializeField] GameObject checkpointWallJump;
    static Vector3 chackpoint = new Vector3(0, 0, 0);
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Start()
    {
        player = GameObject.Find("Player 1");

        if (chackpoint != new Vector3(0, 0, 0)) { player.transform.position = chackpoint; }
        if (HasSpispin) { Instantiate(checkpointSpinspin, transform.position, Quaternion.Euler(0, 0, 0)); }
        if (HasVineWhip) { Instantiate(checkpointVineWhip, transform.position, Quaternion.Euler(0, 0, 0)); }
        if (AcornShild) { Instantiate(checkpointAcornShield, transform.position, Quaternion.Euler(0, 0, 0)); }
        if (walljump) { Instantiate(checkpointWallJump, transform.position, Quaternion.Euler(0, 0, 0)); }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!HasSpispin) { HasSpispin = spinspin.spinspinifikation; }
            if (!HasVineWhip) { HasVineWhip = PlantWhip.Whippywhippy; }
            if (!AcornShild) { AcornShild = AcornShield.AcornShieldw; }
            if (!walljump) { }
            Vector3 chackpoint = collision.transform.position;
            Debug.Log($"spin{HasSpispin}");
        }
    }

}
