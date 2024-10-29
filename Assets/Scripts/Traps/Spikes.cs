using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : Trap
{
    [SerializeField] private float riseHeight = 1f;
    [SerializeField] private float riseSpeed = 2f;
    [SerializeField] private float delayTime = 2f;
    private Vector3 initialPosition;
    private Vector3 raisedPosition;
    private AudioSource SpikesSound;

    new private void Start()
    {
        SpikesSound = GameObject.Find("SpikesSound").GetComponent<AudioSource>();
        initialPosition = transform.position;

        raisedPosition = initialPosition + Vector3.up * riseHeight;

        StartCoroutine(SpikeRoutine());
    }

    new public void OnTriggerEnter2D(Collider2D collision)
    {
        PlayableCharacter character = collision.GetComponent<PlayableCharacter>();
        PlayableCharacter activeCharacter = game.GetCurrentActiveCharacter();

        if (activeCharacter != null && character != null)
        {
            Debug.Log("Character is taking damage: " + activeCharacter.characterName);
            SpikesSound.Play();
            activeCharacter.TakeDamage(howMuchDamage);
        }
    }


    private IEnumerator SpikeRoutine()
    {
        while (true)
        {
            yield return MoveToPosition(raisedPosition);

            yield return new WaitForSeconds(delayTime);

            yield return MoveToPosition(initialPosition);

            yield return new WaitForSeconds(delayTime);
        }
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, riseSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
