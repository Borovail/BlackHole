using System;
using System.Collections;
using Assets.Scripts;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _ambientSource;
    [SerializeField] private AudioSource _sfxSource;
    
    [Header("Audio Clips")]
    public AudioClip MainTheme;
    public AudioClip Ambient;
    public AudioClip Click;
    public AudioClip BlackHole;
    public AudioClip Automation;
    public AudioClip NextStage;
    public AudioClip WinSound;
    
    [Header("Settings")]
    public float FadeDuration = 2f;
    private void Start()
    {
        _ambientSource.clip = Ambient;
        _ambientSource.Play();
        StartCoroutine(PlayMainThemeAfterDelay(5f));
        StartCoroutine(PlayMainThemeAfterDelay(300f));
    }
    
    private IEnumerator PlayMainThemeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        yield return StartCoroutine(FadeIn(_musicSource, FadeDuration));


        PlayMusic();


        yield return new WaitForSeconds(MainTheme.length); // Учитываем время на затухание
        yield return StartCoroutine(FadeOut(_musicSource, FadeDuration));
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
        float targetVolume = 0.5f; // Стандартная громкость

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
        _musicSource.clip = MainTheme;
        _musicSource.Play();
    }

    public void PlaySfx(AudioClip clip)
    {
        _sfxSource.PlayOneShot(clip);
    }
}
