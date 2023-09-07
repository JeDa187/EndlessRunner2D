using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public float minVolume = 0.0001f;
    public float maxVolume = 1.0f;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void SetVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);
        audioSource.volume = volume * maxVolume;
    }
}
