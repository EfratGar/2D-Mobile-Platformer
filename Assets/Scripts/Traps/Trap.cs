using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Trap : MonoBehaviour, IDamagable
{
    [SerializeField] private LayerMask whatIDamage;
    [SerializeField] private int howMuchDamage;

    [SerializeField] private float maxHp;
    private float currentHp;
    private Game game;

    private void Awake()
    {
        game = FindObjectOfType<Game>();

        if (game == null)
        {
            Debug.LogError("Game object not found! Make sure there is a Game object in the scene.");
        }
    }
    void Start()
    {
        currentHp = maxHp;
        game = FindObjectOfType<Game>();
        if (game == null)
        {
            Debug.LogError("Game object not found! Make sure there is a Game object in the scene.");
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        PlayableCharacter activeCharacter = game.GetCurrentActiveCharacter();

        if (activeCharacter == null)
        {
            Debug.LogError("Active character is null! Check your Game class.");
            return;
        }

        float distanceToCharacter = Vector3.Distance(transform.position, activeCharacter.transform.position);

        if (distanceToCharacter < 5f)  // use serilized field
        {
            Debug.Log("Character is taking damage: " + activeCharacter.characterName);
            activeCharacter.TakeDamage(howMuchDamage);  // use applyDamage
        }
        else
        {
            Debug.Log("Character is too far to take damage.");
        }

    }


    public void TakeDamage(float howMuch)
    {
        currentHp -= howMuch;

        if (currentHp <= 0)
        {
            Die();
        }

    }

    public void Die()
    {
        Debug.Log("Trap destroyed!");
        Destroy(gameObject);
    }

    void ApplyDamage(IDamagable damagable)
    {
        damagable.TakeDamage(howMuchDamage);
    }



}
