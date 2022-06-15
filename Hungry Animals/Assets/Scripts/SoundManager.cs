using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles musicSource and effectsSource
/// </summary>
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource effectsSource;

    public void PlaySound(AudioClip clip)
    {
        effectsSource.PlayOneShot(clip);
    }

    public void PlayMusic()
    {
        musicSource.Play();
    }

    public void PauseMusic()
    {
        musicSource.Pause();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public bool IsEffectsSourcePlaying()
    {
        return effectsSource.isPlaying;
    }
}
