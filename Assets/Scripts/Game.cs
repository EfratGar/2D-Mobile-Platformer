using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Game : MonoBehaviour
{
    List<PlayableCharacter> GameCharacters;
    int currentCharacter;

    [SerializeField] private HealthBar healthBar;

    void Awake()
    {
        GameCharacters = FindObjectsOfType<PlayableCharacter>().ToList();
    }

    private void Start()
    {
        currentCharacter = Random.Range(0, 3);
        PlayableCharacter newCharacter = GetCurrentActiveCharacter();
        newCharacter.gameObject.SetActive(true);
        newCharacter.enabled = true;
        
        healthBar.SetCharacter(newCharacter);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchCharacter();
        }
        if (Input.GetKey(KeyCode.Space))
        {
            GetCurrentActiveCharacter().SpecialAbility();
        }

    }

    public void SwitchCharacter()
    {
        PlayableCharacter oldCharacter = GetCurrentActiveCharacter();
        if (!oldCharacter.CharacterIsAlive())
        {
            GameCharacters.Remove(oldCharacter);
        }

        if (GameCharacters.Count == 0) 
        {
            Debug.Log("Game Over!!!!!!");
            return;
        }

        currentCharacter++; 
        if (currentCharacter >= GameCharacters.Count)
        {
            currentCharacter = 0;
        }

        PlayableCharacter NewCharacter = GetCurrentActiveCharacter();

        oldCharacter.gameObject.SetActive(false);
        oldCharacter.GetComponent<Collider2D>().enabled = false;
        oldCharacter.enabled = false;

        NewCharacter.gameObject.SetActive(true);
        NewCharacter.GetComponent<Collider2D>().enabled = true;
        NewCharacter.enabled = true;

        Debug.Log("Active character is now: " + NewCharacter.GetCharacterName());


        PlayerMovement playerMovement = NewCharacter.GetComponent<PlayerMovement>(); 
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }

        healthBar.SetCharacter(NewCharacter);
    }

    public PlayableCharacter GetCurrentActiveCharacter() 
    {
        if (GameCharacters != null && GameCharacters.Count != 0)
        {
            return GameCharacters[currentCharacter];
        }
        else
        {
            return null;
        }

    }
}
