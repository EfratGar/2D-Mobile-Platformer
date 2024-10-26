using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    [SerializeField] private float healthValue;
    private Game game;

    private void Awake()
    {
        game = FindObjectOfType<Game>();

        if (game == null)
        {
            Debug.LogError("Game object not found! Make sure there is a Game object in the scene.");
        }
    }
    void Start() // avoid duplication :)
    {
        game = FindObjectOfType<Game>();
        if (game == null)
        {
            Debug.LogError("Game object not found! Make sure there is a Game object in the scene.");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayableCharacter activeCharacter = game.GetCurrentActiveCharacter();

        if (activeCharacter == null)
        {
            Debug.LogError("Active character is null! Check your Game class.");
            return;
        }

        activeCharacter.AddHealth(healthValue);
        gameObject.SetActive(false);
    }

}


