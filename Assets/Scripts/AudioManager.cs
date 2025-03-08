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
    }
    
    private IEnumerator PlayMainThemeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        PlayMusic();
    }
    
    public void PlayMusic()
    {
        _musicSource.clip = MainTheme;
        _musicSource.Play();
        _musicSource.loop = true;
    }

    public void PlaySfx(AudioClip clip)
    {
        _sfxSource.PlayOneShot(clip);
    }
}
