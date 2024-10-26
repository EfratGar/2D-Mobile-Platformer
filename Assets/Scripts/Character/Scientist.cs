using Cainos.PixelArtPlatformer_VillageProps;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scientist : PlayableCharacter
{
    // Start is called before the first frame update
    public override void SpecialAbility()
    {
        float interactionRange = 0.5f;

        Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(transform.position, interactionRange);

        foreach (Collider2D collider in nearbyColliders)
        {
            Chest chest = collider.GetComponent<Chest>();
            if (chest != null && chest.IsOpened == false)
            {
                chest.IsOpened = true;
                break;
            }
        }
    }

    new

         // Update is called once per frame
         void Update()  // move to base
    {
        base.Update();
        playerMovement.SetSpeed(5);
        playerMovement.SetjumpForce(5);
    }
}

