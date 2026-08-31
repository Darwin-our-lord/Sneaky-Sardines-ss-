using UnityEditor.Sprites;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public GameObject player;
    public float detectionDistance = 5f;
    public LayerMask targetLayer;
    public float outer2 = 15f;
    public float iner2 = 7f;
    public float detectionTime = 1f; // seconds required to trigger
    private float timer = 0f;
    private bool targetDetected = false;
    bool trorfal = false;


    void Update()
    {
        float[] angles = { -outer2, -iner2, 0f, iner2, outer2 };
        bool anyHit = false;
        for (int i = 0; i < angles.Length; i++)
        {
            float angle = angles[i];
            Vector2 direction =
                Quaternion.Euler(0, 0, angle) * transform.up;

            RaycastHit2D hit = Physics2D.Raycast(
                transform.position,
                direction,
                detectionDistance,
                targetLayer
            );

            Debug.DrawRay(
                transform.position,
                direction * detectionDistance,
                angle == 0 ? Color.green : Color.red
            );
            if (spinspin.spinspinifikation)
            {
                if (hit.collider != null)
                {

                    anyHit = true;

                    {
                        Debug.Log("!");
                        if (hit.collider.CompareTag("Player"))
                        {
                            Debug.Log("Detected: " + hit.collider.name);
                            trorfal = true;
                            spinspin.fanfan = true;
                            player.GetComponent<Rigidbody2D>().gravityScale = -1;
                            break;
                        }
                    }
                }
                else if (trorfal == true)
                {
                    trorfal = false;
                    spinspin.fanfan = false;
                    player.GetComponent<Rigidbody2D>().gravityScale = 1;
                }
            }
        }
    }
}
