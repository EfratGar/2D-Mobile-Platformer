using System.Collections;
using UnityEngine;

public class Spiritual : PlayableCharacter, IHideable
{
    private bool isHiding = false;
    private AudioSource VanishingSound;
    private AudioSource UnVanishingSound;
    public bool IsHiding => isHiding;
    new private void Start()
    {
        base.Start();
        VanishingSound = GameObject.Find("VanishingSound").GetComponent<AudioSource>();
        UnVanishingSound = GameObject.Find("UnVanishingSound").GetComponent<AudioSource>();
    }

    new private void OnEnable()
    {
        isHiding = false;
        base.OnEnable();
    }
    public override void SpecialAbility()
    {
        isHiding = !isHiding; // Toggle hiding state

        if (isHiding)
        {
            StartCoroutine(FadeAlpha(0.1f)); // Change to hidden alpha
            VanishingSound.Play();
        }
        else
        {
            StartCoroutine(FadeAlpha(1f)); // Change back to visible alpha
            UnVanishingSound.Play();
        }
    }

    protected override IEnumerator FadeAlpha(float targetAlpha)
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
}
