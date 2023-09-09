using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource, effectsSource;
    [SerializeField] float maxVolume = 1.0f;
    private bool isAudioEnabled = true;

    private void Start()
    {
        musicSource.enabled = isAudioEnabled;
        effectsSource.enabled = isAudioEnabled;
    }

    public void SetMusicVolume(float MusicVolume)
    {
        MusicVolume = Mathf.Clamp01(MusicVolume);
        musicSource.volume = MusicVolume * maxVolume;
    }
    public void SetSFXVolume(float SFXvolume)
    {
        SFXvolume = Mathf.Clamp01(SFXvolume);
        effectsSource.volume = SFXvolume * maxVolume;
    }
    public void AudioToggle()
    {
        isAudioEnabled = !isAudioEnabled; // Toggle the state
        musicSource.enabled = isAudioEnabled; // Apply the new state to the AudioSource
        effectsSource.enabled = isAudioEnabled; // Apply the new state to the AudioSource
    }
}
