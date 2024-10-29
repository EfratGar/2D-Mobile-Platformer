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
    private AudioSource KnightSwordSlash;
    private AudioSource KnightGhostSound;
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
        KnightSwordSlash = GameObject.Find("KnightSwordSlash").GetComponent<AudioSource>();
        KnightGhostSound = GameObject.Find("KnightGhostSound").GetComponent<AudioSource>();
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

        if (distanceToCharacter < 4f && !CharacterIsHidden())
        {
            Debug.Log("Character is taking damage: " + activeCharacter.characterName);
            anim.SetTrigger("attack");
            KnightSwordSlash.Play();
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

        if (coolDownTimer >= attackCooldown)
        {
            coolDownTimer = 0;
        }
        AdjustGhostSoundVolume();
    }
    private void AdjustGhostSoundVolume()
    {
        PlayableCharacter activeCharacter = game.GetCurrentActiveCharacter();

        if (activeCharacter != null)
        {
            float distanceToCharacter = Vector3.Distance(transform.position, activeCharacter.transform.position);

            float maxDistance = 50f; 
            float minDistance = 3f;   

            float volume = Mathf.Clamp01(1 - (distanceToCharacter - minDistance) / (maxDistance - minDistance));

            KnightGhostSound.volume = volume;
        }
    }

    private bool CharacterInSight()
    {
        Vector3 boxCenter = knightBoxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance;
        Vector3 boxSize = new Vector3(knightBoxCollider.bounds.size.x * range, knightBoxCollider.bounds.size.y, knightBoxCollider.bounds.size.z);

        RaycastHit2D[] hits = Physics2D.BoxCastAll(boxCenter, boxSize, 0, Vector2.left, 0, characterLayer);

        foreach (RaycastHit2D hit in hits)
        {
            IHideable hideableCharacter = hit.collider.GetComponent<IHideable>();
            if (hideableCharacter != null && hideableCharacter.IsHiding)
            {
                continue;
            }

            if (hit.collider.GetComponent<PlayableCharacter>() != null)
            {
                return true; 
            }
        }

        return false;
    }



    private bool CharacterIsHidden()
    {
        IHideable activeCharacter = game.GetCurrentActiveCharacter() as IHideable;
        return activeCharacter != null && activeCharacter.IsHiding;
    }

    private void DamageCharacter()
    {
        PlayableCharacter activeCharacter = game.GetCurrentActiveCharacter();

        if (activeCharacter != null)
        {
            activeCharacter.TakeDamage(damage);
            Debug.Log(activeCharacter.characterName + " took " + damage + " damage.");
        }
    }


    private void OnDrawGizmos() // what is this doing?
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(knightBoxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(knightBoxCollider.bounds.size.x * range, knightBoxCollider.bounds.size.y, knightBoxCollider.bounds.size.z));
    }

}

