using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cainos.LucidEditor;

namespace Cainos.PixelArtPlatformer_VillageProps
{
    public class Chest : MonoBehaviour
    {
        [FoldoutGroup("Reference")]
        public Animator animator;

        [FoldoutGroup("Reference")]
        [SerializeField] public GameObject Key;

        [FoldoutGroup("Reference")]
        private AudioSource ChestOpeningSound;

        [FoldoutGroup("Runtime"), ShowInInspector, DisableInEditMode]
        public bool IsOpened
        {
            get { return isOpened; }
            set
            {
                isOpened = value;
                animator.SetBool("IsOpened", isOpened);

                if (isOpened)
                {
                    ShowTreasure();
                }
            }
        }
        private void Start()
        {
            ChestOpeningSound = GameObject.Find("ChestOpeningSound").GetComponent<AudioSource>();
        }

        private bool isOpened;

        [FoldoutGroup("Runtime"), Button("Open"), HorizontalGroup("Runtime/Button")]
        public void Open()
        {
            IsOpened = true;
        }

        [FoldoutGroup("Runtime"), Button("Close"), HorizontalGroup("Runtime/Button")]
        public void Close()
        {
            IsOpened = false;
        }

        private void ShowTreasure()
        {
            if (Key != null)
            {
                Key.SetActive(true); 
            }

            if (ChestOpeningSound != null)
            {
                ChestOpeningSound.Play(); 
            }
        }
    }
    
}
