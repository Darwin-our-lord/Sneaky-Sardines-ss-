using UnityEngine;

public class PlantWhip : GraftablePart
{
    public static bool Whippywhippy = false;
    [SerializeField] AudioClip AttachSound; // The sound of the player attaching the whip
    [SerializeField] AchievementScreenManager achievementScreenManager; // Reference to the AchievementScreenManager script on canvas
    private void Start()
    {
        Whippywhippy = false;
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!held && collision.transform.gameObject.CompareTag("Player"))
        {
            achievementScreenManager.UnlockNewAbility("LeafAttack");
            Whippywhippy = true;
            held = true;
            transform.position = collision.transform.position;
            collision.gameObject.GetComponent<LeafAttack>().held = true; // Set the static variable to true when the whip is picked up
            AudioSource.PlayClipAtPoint(AttachSound, transform.position, 10f);

            Destroy(this.gameObject);
        }
    }
}
