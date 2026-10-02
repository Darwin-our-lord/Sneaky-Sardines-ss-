using System.Collections.Generic;
using UnityEngine;

public class chechPoint : MonoBehaviour
{
    public GameObject spinspinw;
    public GameObject vinewhipw;
    public GameObject acornshieldw;
    public GameObject walljumpw;
    static chechPoint instance;
    // er her bare glem dem
    public bool mHasSpispin = false;
    public bool mHasVineWhip = false;
    public bool mHasAcornShild = false;
    public bool mHasWalljump = false;
    // HUSK IGEN
    public static bool HasSpispin = false;
    public static bool HasVineWhip = false;
    public static bool HasAcornShild = false;
    public static bool HasWalljump = false;

    [SerializeField] GameObject checkpointSpinspin;
    [SerializeField] GameObject checkpointVineWhip;
    [SerializeField] GameObject checkpointAcornShield;
    [SerializeField] GameObject checkpointWallJump;
    public static Vector3 chackpoint = new Vector3(0, 0, 0);

    // Start is called once be  fore the first execution of Update after the MonoBehaviour is created
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
                if (HasAcornShild) { Destroy(obj); }
            }
            else if (obj.name.Contains("qeaigr,awugr"))
            {
                if (HasWalljump) { Destroy(obj); } 
            }
        }

    }
    private void Update()
    {

    }
    private void Start()
    {

        if (HasSpispin) { Instantiate(checkpointSpinspin, transform.position, Quaternion.Euler(0, 0, 0)); }
        if (HasVineWhip) { Instantiate(checkpointVineWhip, transform.position, Quaternion.Euler(0, 0, 0)); }
        if (HasAcornShild) { Instantiate(checkpointAcornShield, transform.position, Quaternion.Euler(0, 0, 0)); }
        if (HasWalljump) { Instantiate(checkpointWallJump, transform.position, Quaternion.Euler(0, 0, 0)); }
        if (chackpoint == new Vector3(0, 0, 0)) { Debug.Log("hvis dette viser og ikke i starten er checkpoints fucked"); } else { transform.position = chackpoint; }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("checkpoint"))
        {
            if (!HasSpispin) { HasSpispin = spinspin.spinspinifikation; mHasSpispin = HasSpispin; }
            if (!HasVineWhip) { HasVineWhip = PlantWhip.Whippywhippy; mHasVineWhip = HasVineWhip; }
            if (!HasAcornShild) { HasAcornShild = AcornShield.AcornShieldw; mHasAcornShild = HasAcornShild;  }
            if (!HasWalljump) { }
            chackpoint = transform.position; 
            Debug.Log($"spin{HasSpispin}");
        }
    }

}
