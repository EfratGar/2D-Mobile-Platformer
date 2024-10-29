using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Trap : MonoBehaviour, IDamagable
{
    [SerializeField] private LayerMask whatIDamage;
    [SerializeField] protected int howMuchDamage;

    [SerializeField] protected float maxHp;
    protected float currentHp;
    protected Game game;
    [SerializeField] float distanceToCharacter;
    private Animator anim;

    private void Awake()
    {
        game = FindObjectOfType<Game>();

        if (game == null)
        {
            Debug.LogError("Game object not found! Make sure there is a Game object in the scene.");
        }
    }
    public void Start()
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
        PlayableCharacter character = collision.GetComponent<PlayableCharacter>();
        PlayableCharacter activeCharacter = game.GetCurrentActiveCharacter();

        if (activeCharacter != null && character != null)
        {
            Debug.Log("Character is taking damage: " + activeCharacter.characterName);
            activeCharacter.TakeDamage(howMuchDamage);
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
        else
        {
            Debug.Log("Trap took damage, remaining HP: " + currentHp);
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
