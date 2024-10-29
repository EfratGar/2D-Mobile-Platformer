using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyCollectible : MonoBehaviour
{
    [SerializeField] public GameObject KeyUI;
    private bool isCollected = false;
    private AudioSource PickKeySound;
    private Game game;

    private void Start()
    {
        game = FindObjectOfType<Game>();
        if (KeyUI != null)
        {
            KeyUI.SetActive(false);
        }
        PickKeySound = GameObject.Find("PickKeySound").GetComponent<AudioSource>();

        if (PickKeySound == null)
        {
            Debug.LogError("PickKeySound object not found or AudioSource component is missing!");
        }
    }


    public void CollectKey()
    {
        if (isCollected)
        {
            return;
        }

        isCollected = true;
        gameObject.SetActive(false);
        Debug.Log("Key Collected - Playing Sound"); 
        PickKeySound.Play();

        if (KeyUI != null)
        {
            KeyUI.SetActive(true);
        }
    }

}
