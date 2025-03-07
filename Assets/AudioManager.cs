using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    
    [Header("Audio Clips")]
    public AudioClip mainTheme;
    public AudioClip ambient;
    public AudioClip click;
    public AudioClip blackHole;
    public AudioClip automation;
    public AudioClip nextStage;

    private void Start()
    {
        musicSource.clip = ambient;
        musicSource.Play();
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
