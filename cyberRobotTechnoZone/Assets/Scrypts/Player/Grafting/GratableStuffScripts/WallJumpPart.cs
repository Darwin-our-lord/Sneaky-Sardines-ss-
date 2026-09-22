using UnityEngine;

public class WallJumpPart : GraftablePart
{
    PlayerMovement playerMovement;
    // Update is called once per frame
    void Update()
    {
        if (held)
        {
            if (playerMovement == null) playerMovement = transform.GetComponentInParent<PlayerMovement>();
            if (Input.GetKeyDown(KeyCode.Space) && !playerMovement.grounded)
            {




            }
        }

    }
}
