using System;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource ambientSource;
    [SerializeField] private AudioSource sfxSource;
    
    [Header("Audio Clips")]
    public AudioClip mainTheme;
    public AudioClip ambient;
    public AudioClip click;
    public AudioClip blackHole;
    public AudioClip automation;
    public AudioClip nextStage;
    
    [Header("Settings")]
    public float fadeDuration = 2f;
    private void Start()
    {
        ambientSource.clip = ambient;
        ambientSource.Play();
        StartCoroutine(PlayMainThemeAfterDelay(5f));
        StartCoroutine(PlayMainThemeAfterDelay(300f));
    }
    
    private IEnumerator PlayMainThemeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        yield return StartCoroutine(FadeIn(musicSource, fadeDuration));


        PlayMusic();


        yield return new WaitForSeconds(mainTheme.length); // Учитываем время на затухание
        yield return StartCoroutine(FadeOut(musicSource, fadeDuration));
    }

    private IEnumerator FadeOut(AudioSource source, float duration)
    {
        float startVolume = source.volume;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, 0, time / duration);
            yield return null;
        }

        source.volume = 0;
    }

    private IEnumerator FadeIn(AudioSource source, float duration)
    {
        float time = 0;
        source.volume = 0;
        float targetVolume = 0.1f; // Стандартная громкость

        while (time < duration)
        {
            time += Time.deltaTime;
            source.volume = Mathf.Lerp(0, targetVolume, time / duration);
            yield return null;
        }

        source.volume = targetVolume;
    }
    
    public void PlayMusic()
    {
        musicSource.clip = mainTheme;
        musicSource.Play();
    }

    public void PlaySfx(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}
