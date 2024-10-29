using System.Collections;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightSource : MonoBehaviour
{
    private Light2D lightComponent;
    private Animator anim;
    [SerializeField] private float lightIntensity;
    [SerializeField] private float fadeDuration;
    private AudioSource LampLightingSound;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        lightComponent = GetComponent<Light2D>();
        lightComponent.intensity = 0;
    }
    private void Start()
    {
        LampLightingSound = GameObject.Find("LampLightingSound").GetComponent<AudioSource>();
    }
    public void ToggleLight()
    {
        anim.SetTrigger("lighting");
        LampLightingSound.Play();
        StartCoroutine(FadeInLight());

    }

    private IEnumerator FadeInLight()
    {
        float elapsedTime = 0f;
        float startIntensity = lightComponent.intensity;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            lightComponent.intensity = Mathf.Lerp(startIntensity, lightIntensity, elapsedTime / fadeDuration);
            yield return null;
        }

        lightComponent.intensity = lightIntensity;
    }

}
