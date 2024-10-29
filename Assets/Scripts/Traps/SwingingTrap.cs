using System.Collections;
using UnityEngine;

public class SwingingTrap : Trap
{
    [SerializeField] private float swingAngle = 45f;
    [SerializeField] private float swingSpeed = 1f;

    private Quaternion startRotation;
    private Quaternion endRotation;
    private Animator animator;
    private AudioSource ChainsSound;
    [SerializeField] private float maxVolumeDistance = 3f;
    [SerializeField] private float minVolumeDistance = 10f;

    new private void Start()
    {
        base.Start();
        startRotation = Quaternion.Euler(0, 0, swingAngle);
        endRotation = Quaternion.Euler(0, 0, -swingAngle);

        animator = GetComponent<Animator>();
        ChainsSound = GameObject.Find("ChainsSound").GetComponent<AudioSource>();
    }

    private void Update()
    {
        transform.rotation = Quaternion.Lerp(startRotation, endRotation, Mathf.PingPong(Time.time * swingSpeed, 1));
        AdjustVolumeBasedOnDistance();
    }

    private void AdjustVolumeBasedOnDistance()
    {
        PlayableCharacter activeCharacter = FindObjectOfType<Game>().GetCurrentActiveCharacter();

        if (activeCharacter == null) return;

        float distance = Vector3.Distance(transform.position, activeCharacter.transform.position);

        if (distance <= maxVolumeDistance)
        {
            ChainsSound.volume = 1f;
        }
        else if (distance >= minVolumeDistance)
        {
            ChainsSound.volume = 0f;
        }
        else
        {
            float t = 1 - ((distance - maxVolumeDistance) / (minVolumeDistance - maxVolumeDistance));
            ChainsSound.volume = Mathf.Lerp(0f, 1f, t);
        }
    }
}
