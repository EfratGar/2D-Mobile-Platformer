using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class KnightEnemy : MonoBehaviour, IDamagable
{
    [Header("Name")]
    [SerializeField] public string characterName;

    [Header("Health")]
    [SerializeField] public float maxHp;
    [SerializeField] private LayerMask whatIDamage;
    [SerializeField] private int howMuchDamage;
    public float currentHp { get; private set; }

    private Animator anim;
    [SerializeField] private float attackCooldown;
    [SerializeField] private int damage;
    [SerializeField] private float range;
    [SerializeField] private float colliderDistance;
    [SerializeField] private LayerMask characterLayer;
    [SerializeField] private BoxCollider2D knightBoxCollider;
    private float coolDownTimer = Mathf.Infinity;
    private bool dead;
    private Game game;

    private void Awake()
    {
        currentHp = maxHp;
        game = FindObjectOfType<Game>();
        anim = GetComponent<Animator>();
        knightBoxCollider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        game = FindObjectOfType<Game>();
    }

    public void OnTriggerEnter2D(Collider2D collision)  // Thats duplicates DamageCharacter
    {
        PlayableCharacter activeCharacter = game.GetCurrentActiveCharacter();

        if (activeCharacter == null)
        {
            Debug.LogError("Active character is null! Check your Game class.");
            return;
        }

        float distanceToCharacter = Vector3.Distance(transform.position, activeCharacter.transform.position);

        if (distanceToCharacter < 4f)
        {
            Debug.Log("Character is taking damage: " + activeCharacter.characterName);
            anim.SetTrigger("attack");
        }
        else
        {
            Debug.Log("Character is too far to take damage.");
        }

    }
    public void TakeDamage(float howMuch)  // trigger this in the correct time (in projectile i guess) thats fun stuff
    {
        currentHp = Mathf.Clamp(currentHp - howMuch, 0, maxHp);

        if (currentHp > 0)
        {
            anim.SetTrigger("hurt");
        }
        else
        {
            if (!dead)
            {
                Die();
            }
        }
    }
    public void Die()
    {
        anim.SetTrigger("die");
        dead = true;
        StartCoroutine(RemoveEnemyAfterDeath());

    }

    private IEnumerator RemoveEnemyAfterDeath()
    {
        // Waiting for animation ending
        yield return new WaitForSeconds(1.5f);

        gameObject.SetActive(false); // Enemy is hidden after death
    }

    public void Update()
    {
        coolDownTimer += Time.deltaTime;

        //Attack only if player is in sight
        if (CharacterInSight())
        {
            if (coolDownTimer >= attackCooldown)
            {
                coolDownTimer = 0;
                
            }
        }
    }

    private bool CharacterInSight()
    {
        // simplify the hell of this
        // Use BoxCast instead of BoxCastAll
        RaycastHit2D[] hits = Physics2D.BoxCastAll(knightBoxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(knightBoxCollider.bounds.size.x * range, knightBoxCollider.bounds.size.y, knightBoxCollider.bounds.size.z), 0, Vector2.left, 0, characterLayer);

        foreach (RaycastHit2D hit in hits)
        {
            PlayableCharacter character = hit.collider.GetComponent<PlayableCharacter>();
            if (character != null)
            {
                return true;
            }
        }

        return false;
    }


    private void DamageCharacter()
    {
        if (CharacterInSight()) // thats double check for charachter in sight
        {

            RaycastHit2D[] hits = Physics2D.BoxCastAll(knightBoxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
                new Vector3(knightBoxCollider.bounds.size.x * range, knightBoxCollider.bounds.size.y, knightBoxCollider.bounds.size.z), 0, Vector2.left, 0, characterLayer);

            foreach (RaycastHit2D hit in hits)
            {
                PlayableCharacter character = hit.collider.GetComponent<PlayableCharacter>();
                if (character != null)
                {
                    PlayableCharacter activeCharacter = game.GetCurrentActiveCharacter();
                    activeCharacter.TakeDamage(damage);
                    Debug.Log(activeCharacter.characterName + " Got hit");
                }
            }
        }
    }


    private void OnDrawGizmos() // what is this doing?
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(knightBoxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(knightBoxCollider.bounds.size.x * range, knightBoxCollider.bounds.size.y, knightBoxCollider.bounds.size.z));
    }

}

