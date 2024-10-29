using Cainos.PixelArtPlatformer_VillageProps;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Scientist : PlayableCharacter
{
    [SerializeField] float interactionRange;

    public override void SpecialAbility()
    {

        Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(transform.position, interactionRange);

        foreach (Collider2D collider in nearbyColliders)
        {
            Chest chest = collider.GetComponent<Chest>();
            if (chest != null && chest.IsOpened == false)
            {
                chest.IsOpened = true;
            }

            LightSource CeilingLight = collider.GetComponent<LightSource>(); 
            if (CeilingLight != null)
            {
                CeilingLight.ToggleLight(); 
            }

            KeyCollectible keyCollectible = collider.GetComponent<KeyCollectible>();
            if (keyCollectible != null)
            {
                keyCollectible.CollectKey();
            }

        }
    }
}

