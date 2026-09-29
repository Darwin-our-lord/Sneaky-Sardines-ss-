using UnityEngine;
using UnityEngine.InputSystem;

public class PlatformCollision : MonoBehaviour
{
    private InputActionMap actionMap;
    private InputAction moveAction;
    private Collider2D collider;
    private PlatformEffector2D effector;
    private bool isOnPlatform;
    private void Awake()
    {
        actionMap = new InputActionMap("Platform");

        moveAction = actionMap.AddAction("Down");
        moveAction.AddBinding("<Keyboard>/s");
        moveAction.AddBinding("<Keyboard>/downArrow");
        collider = GetComponent<Collider2D>();
        actionMap.Enable();
    }
    private void Update()
    {
        if (moveAction.IsPressed() && isOnPlatform == true)
        {
            collider.isTrigger = true;
            gameObject.layer = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isOnPlatform = true;

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isOnPlatform = false;
        gameObject.layer = 3;

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        
        isOnPlatform = false;
        collider.isTrigger = false;
        gameObject.layer = 3;
    }
}
