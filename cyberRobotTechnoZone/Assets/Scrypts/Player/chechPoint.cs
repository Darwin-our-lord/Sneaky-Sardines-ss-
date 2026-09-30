using UnityEngine;

public class chechPoint : MonoBehaviour
{
    public GameObject spinspinw;
    public GameObject vinewhipw;
    public GameObject acornshieldw;
    public GameObject walljumpw;
    static chechPoint instance;
    public GameObject player;
    public  bool HasSpispin = false;
    public  bool HasVineWhip = false;
    public  bool AcornShild = false;
    public  bool walljump = false;
    [SerializeField] GameObject checkpointSpinspin;
    [SerializeField] GameObject checkpointVineWhip;
    [SerializeField] GameObject checkpointAcornShield;
    [SerializeField] GameObject checkpointWallJump;
    public  Vector3 chackpoint = new Vector3(0, 0, 0);

    // Start is called once be  fore the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    

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
    private void Update()
    {
        if (transform.parent== null)
        {
        transform.position = player.transform.position;
        transform.parent = player.transform;
            if (transform.parent != null)
            {
                if (HasSpispin) { Instantiate(checkpointSpinspin, transform.position, Quaternion.Euler(0, 0, 0)); }
                if (HasVineWhip) { Instantiate(checkpointVineWhip, transform.position, Quaternion.Euler(0, 0, 0)); }
                if (AcornShild) { Instantiate(checkpointAcornShield, transform.position, Quaternion.Euler(0, 0, 0)); }
                if (walljump) { Instantiate(checkpointWallJump, transform.position, Quaternion.Euler(0, 0, 0)); }
                if (chackpoint == new Vector3(0, 0, 0)) { Debug.Log("hvis dette viser og ikke i starten er checkpoints fucked"); } else { transform.position = chackpoint; }
            }
        }

    }
    private void Start()
    {


    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("checkpoint"))
        {
            if (!HasSpispin) { HasSpispin = spinspin.spinspinifikation; }
            if (!HasVineWhip) { HasVineWhip = PlantWhip.Whippywhippy; }
            if (!AcornShild) { AcornShild = AcornShield.AcornShieldw; }
            if (!walljump) { }
            if (checkpointTP.check.Contains(collision.gameObject)) { chackpoint = player.transform.position; }
            Debug.Log($"spin{HasSpispin}");
        }
    }

}
