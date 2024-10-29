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
    [SerializeField] private int characterSpeed;
    [SerializeField] private int characterJumpForce;

    [Header("Sprite")]
    [SerializeField] protected Sprite characterSprite;
    [Header("Animator")]
    [SerializeField] protected RuntimeAnimatorController characterAnimator;
    private AudioSource MalePainSound;
    private AudioSource FemalePainSound;
    [Header("Character")]
    [SerializeField] protected Vector3 characterScale = new Vector3(1, 1, 1);
    [SerializeField] protected float characterAlpha = 1f;
    [SerializeField] protected Vector2 colliderSize;
    [SerializeField] protected Vector2 colliderOffset;

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;


    private BoxCollider2D boxCollider;
    protected SpriteRenderer spriteRenderer;
    private Color characterColor;
    bool InFlashing = false;

    private bool dead;

    private Game game;

    protected void Start()
    {
        game = FindObjectOfType<Game>();

    }


    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        MalePainSound = GameObject.Find("MalePainSound").GetComponent<AudioSource>();
        FemalePainSound = GameObject.Find("FemalePainSound").GetComponent<AudioSource>();
        characterColor = spriteRenderer.color;


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
        if (InFlashing)
        {
            return;
        }
        currentHp = Mathf.Clamp(currentHp - howMuch, 0, maxHp);

        if (currentHp > 0)
        {
            playerMovement.GetAnimator().SetTrigger("hurt");
            StartCoroutine(inVulnerability());
            if (characterName == "Warrior" || characterName == "Spiritual")
            {
                MalePainSound.Play();
            }
            else
            {
                FemalePainSound.Play();
            }
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
        if (game == null)
        {
            Debug.LogError("game is NULL@@@!!! just before game is needed");
            yield break;
        }
        game.SwitchCharacter();
        //gameObject.SetActive(false); // Character is hidden after death
    }

    private void AdjustCollider()
    {
        Bounds spriteBounds = characterSprite.bounds;
        Vector2 currentVelocity = playerMovement.body.velocity;
        boxCollider.size = new Vector2(spriteBounds.size.x, spriteBounds.size.y);
        boxCollider.offset = new Vector2(spriteBounds.center.x, spriteBounds.center.y);
        playerMovement.body.velocity = currentVelocity;
    }

    protected void OnEnable()
    {
        if (playerMovement == null)
        {
            return;
        }

        characterColor.a = characterAlpha;
        spriteRenderer.color = characterColor;
        InFlashing = false;

        spriteRenderer.sprite = characterSprite;
        playerMovement.SetAnimator(characterAnimator);
        transform.localScale = characterScale;
        AdjustCollider();

        playerMovement.SetSpeed(characterSpeed);
        playerMovement.SetjumpForce(characterJumpForce);

    }

    private IEnumerator inVulnerability()
    {
        InFlashing = true;
        Color currentColor = spriteRenderer.color;
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRenderer.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRenderer.color = currentColor;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }
        InFlashing = false;

    }

    protected virtual IEnumerator FadeAlpha(float targetAlpha)
    {
        float currentAlpha = spriteRenderer.color.a;
        float duration = 0.5f;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(currentAlpha, targetAlpha, elapsedTime / duration);
            Color newColor = spriteRenderer.color;
            newColor.a = newAlpha;
            spriteRenderer.color = newColor;

            yield return null;
        }
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
