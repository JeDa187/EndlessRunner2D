using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    public const string MIXER_MUSIC = "MusicVolume";
    public const string MIXER_SFX = "SFXVolume";

    private void Awake()
    {
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    private void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat(SoundManager.MUSIC_KEY, 1f);
        sfxSlider.value = PlayerPrefs.GetFloat(SoundManager.SFX_KEY, 1f);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(SoundManager.MUSIC_KEY, musicSlider.value);
        PlayerPrefs.SetFloat(SoundManager.SFX_KEY, sfxSlider.value);
    }

    private void SetMusicVolume(float value)
    {
        mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(value) * 20);
    }

    private void SetSFXVolume(float value)
    {
        mixer.SetFloat(MIXER_SFX, Mathf.Log10(value) * 20);
    }

}
