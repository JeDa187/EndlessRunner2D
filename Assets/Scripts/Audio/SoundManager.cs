using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField]
    AudioMixer mixer;

    [SerializeField]
    private AudioSource _musicSource, _effectsSource;
    //[SerializeField]
    //private AudioSource playOnClickSource;

    [SerializeField]
    List<AudioClip> audioClips = new();

    public const string MUSIC_KEY = "musicVolume";
    public const string SFX_KEY = "sfxVolume";


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        LoadVolume();
    }

    //public void PlaySound(AudioClip clip)
    //{
    //    playOnClickSource.PlayOneShot(clip);
    //}

    public void ChangeMasterVolume(float value)
    {
        AudioListener.volume = value;
    }

    public void ToggleEffects()
    {
        _effectsSource.mute = !_effectsSource.mute;
    }

    public void ToggleMusic()
    {
        _musicSource.mute = !_musicSource.mute;
    }

    private void LoadVolume()
    {
        float musicVolume = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
        float sfxVolume = PlayerPrefs.GetFloat(SFX_KEY, 1f);

        mixer.SetFloat(VolumeSettings.MIXER_MUSIC, Mathf.Log10(musicVolume) * 20);
        mixer.SetFloat(VolumeSettings.MIXER_MUSIC, Mathf.Log10(sfxVolume) * 20);
    }

}
