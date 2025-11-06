using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Slider musicSlider, sfxSlider;
    private void Start()
    {
        // Initialize sliders with saved values or defaults
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
    }

    public void OnMusicVolumeChanged(float value)
    {
        AudioManager.Instance.MusicControl(value);
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public void OnSFXVolumeChanged(float value)
    {
        AudioManager.Instance.SFXControl(value);
        PlayerPrefs.SetFloat("SFXVolume", value);
    }
}
