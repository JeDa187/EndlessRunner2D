using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public Slider volumeSlider;
    public Slider SFXSlider;
    public AudioManager audioManager;

    private void Start()
    {
        volumeSlider.onValueChanged.AddListener(ChangeMusicVolume);
        SFXSlider.onValueChanged.AddListener(ChangeSFXVolume);
    }

    private void ChangeMusicVolume(float volume)
    {
        audioManager.SetVolume(volume);
    }
    private void ChangeSFXVolume(float SFXvolume)
    {
        audioManager.SetVolume(SFXvolume);
    }
}
