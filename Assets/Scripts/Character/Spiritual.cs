using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spiritual : PlayableCharacter
{
    public override void SpecialAbility()
    {
        Debug.Log("Spiritual is hiding!");
    }
    new

         // Update is called once per frame
         void Update()  // move to base
    {
        base.Update();
        playerMovement.SetSpeed(2);
        playerMovement.SetjumpForce(2);
    }
}
