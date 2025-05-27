using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    public void SetMasterVolume()
    {
        float volume = masterSlider.value;
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume)*20);
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume()
    {
        // Implement SFX volume control
        // float volume = sfxSlider.value;
        // Example: audioMixer.SetFloat("SFXVolume", Mathf.Log10(sfxSlider.value) * 20);
    }
}
