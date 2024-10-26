using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public abstract class PlayableCharacter : MonoBehaviour, IDamagable
{
    [SerializeField] public string characterName;

    [Header("Health")]
    [SerializeField] public float maxHp = 100f;
    public float currentHp { get; private set; }

    [Header("Movement")]
    [SerializeField] protected PlayerMovement playerMovement;

    [Header("Sprite")]
    [SerializeField] protected Sprite characterSprite;
    [Header("Animator")]
    [SerializeField] protected RuntimeAnimatorController characterAnimator;
    [Header("Character")]
    [SerializeField] protected Vector3 characterScale = new Vector3(1, 1, 1);
    [SerializeField] protected float characterAlpha = 1f;
    [SerializeField] protected Vector2 colliderSize;
    [SerializeField] protected Vector2 colliderOffset;

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;


    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;
    private bool dead;

    public Game game;

    private void Start()
    {
        game = FindObjectOfType<Game>();
    }


    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();


        currentHp = maxHp;

        if (string.IsNullOrEmpty(characterName))
        {
            characterName = gameObject.name;
        }
    }
    public string GetCharacterName()
    {
        return characterName;
    }

    public void TakeDamage(float howMuch)
    {
        currentHp = Mathf.Clamp(currentHp - howMuch, 0, maxHp);

        if (currentHp > 0)
        {
            playerMovement.GetAnimator().SetTrigger("hurt");
            StartCoroutine(inVulnerability());
        }
        else
        {
            if (!dead)
            {
                Die();
            }
        }
    }

    public void AddHealth(float _value)
    {
        currentHp = Mathf.Clamp(currentHp + _value, 0, maxHp);
    }

    public abstract void SpecialAbility();
    void ApplyDamage(IDamagable target)
    {
        
    }

    public void Die()
    {
        dead = true;
        playerMovement.GetAnimator().SetTrigger("die");
        GetComponent<PlayerMovement>().enabled = false;

        StartCoroutine(RemoveCharacterAfterDeath());

    }

    private IEnumerator RemoveCharacterAfterDeath()
    {
        // Waiting for animation ending
        yield return new WaitForSeconds(1.5f);

        gameObject.SetActive(false); // Character is hidden after death
        game.SwitchCharacter();
    }

    private void AdjustCollider()
    {
        Bounds spriteBounds = characterSprite.bounds;
        Vector2 currentVelocity = playerMovement.body.velocity;
        boxCollider.size = new Vector2(spriteBounds.size.x, spriteBounds.size.y);
        boxCollider.offset = new Vector2(spriteBounds.center.x, spriteBounds.center.y);
        playerMovement.body.velocity = currentVelocity;
    }

    private void OnEnable()
    {
        if (playerMovement == null)
        {
            return;
        }

        spriteRenderer.sprite = characterSprite;
        playerMovement.SetAnimator(characterAnimator);
        transform.localScale = characterScale;
        AdjustCollider();
        Color newColor = spriteRenderer.color;
        newColor.a = characterAlpha;
        spriteRenderer.color = newColor;
        //?

    }

    private IEnumerator inVulnerability() // fix
    {
        Color currentColor = spriteRenderer.color;
        Physics2D.IgnoreLayerCollision(10, 11, true);
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRenderer.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRenderer.color = currentColor;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }

        Physics2D.IgnoreLayerCollision(10, 11, false);
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            TakeDamage(10);
    }

    public bool CharacterIsAlive()
    {
        if (currentHp > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


}
