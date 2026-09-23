using UnityEngine;

public class chechPoint : MonoBehaviour
{
    public GameObject spinspinw;
    public GameObject vinewhipw;
    public GameObject acornshieldw;
    public GameObject walljumpw;


    public static bool HasSpispin = false;
    public static bool HasVineWhip = false;
    public static bool AcornShild = false;
    public static bool walljump = false;
    [SerializeField] GameObject checkpointSpinspin;
    [SerializeField] GameObject checkpointVineWhip;
    [SerializeField] GameObject checkpointAcornShield;
    [SerializeField] GameObject checkpointWallJump;
    public static Vector3 chackpoint = new Vector3(0, 0, 0);
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        GameObject[] objects = FindObjectsByType<GameObject>();

        foreach (GameObject obj in objects)
        {
            if (obj.name.Contains("Helicop"))
            {
                if (HasSpispin) { Destroy(obj); }
            }
            else if (obj.name.Contains("PlantW"))
            {
                if (HasVineWhip) { Destroy(obj); }
            }
            else if (obj.name.Contains("AcornSh"))
            {
                if (AcornShild) { Destroy(obj); }
            }
            else if (obj.name.Contains("qeaigr,awugr"))
            {
                if (walljump) { Destroy(obj); }
            }
        }

    }
    private void Start()
    {

        if (HasSpispin) { Instantiate(checkpointSpinspin, transform.position, Quaternion.Euler(0, 0, 0)); }
        if (HasVineWhip) { Instantiate(checkpointVineWhip, transform.position, Quaternion.Euler(0, 0, 0)); }
        if (AcornShild) { Instantiate(checkpointAcornShield, transform.position, Quaternion.Euler(0, 0, 0)); }
        if (walljump) { Instantiate(checkpointWallJump, transform.position, Quaternion.Euler(0, 0, 0)); }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("checkpoint"))
        {
            if (!HasSpispin) { HasSpispin = spinspin.spinspinifikation; }
            if (!HasVineWhip) { HasVineWhip = PlantWhip.Whippywhippy; }
            if (!AcornShild) { AcornShild = AcornShield.AcornShieldw; }
            if (!walljump) { }
            chackpoint = transform.position;
            Debug.Log($"spin{HasSpispin}");
        }
    }

}
