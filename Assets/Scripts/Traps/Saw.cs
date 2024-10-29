using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : Trap 
{
    [SerializeField] private float movementDistance;
    [SerializeField] private float movementSpeed;
    private bool movingLeft;
    private float leftEdge;
    private float rightEdge;

    [SerializeField] private AudioSource sawAudioSource; 
    [SerializeField] private float maxVolumeDistance = 3f; 
    [SerializeField] private float minVolumeDistance = 10f;
    private void Awake()
    {
        leftEdge = transform.position.x - movementDistance;
        rightEdge = transform.position.x + movementDistance;
    }
    private void Update()
    {

        if (movingLeft) // simplify !!
        {
            if (transform.position.x > leftEdge)
            {
                transform.position = new Vector3(transform.position.x - movementSpeed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else
                movingLeft = false;

        }
        else
        {
            if (transform.position.x < rightEdge)
            {
                transform.position = new Vector3(transform.position.x + movementSpeed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else
                movingLeft = true;
        }

        AdjustVolumeBasedOnDistance();
    }

    private void AdjustVolumeBasedOnDistance()
    {
        PlayableCharacter activeCharacter = FindObjectOfType<Game>().GetCurrentActiveCharacter();

        if (activeCharacter == null) return;

        float distance = Vector3.Distance(transform.position, activeCharacter.transform.position);

        if (distance <= maxVolumeDistance)
        {
            sawAudioSource.volume = 1f; 
        }
        else if (distance >= minVolumeDistance)
        {
            sawAudioSource.volume = 0f; 
        }
        else
        {
            float t = 1 - ((distance - maxVolumeDistance) / (minVolumeDistance - maxVolumeDistance));
            sawAudioSource.volume = Mathf.Lerp(0f, 1f, t);
        }
    }

}




