using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public Sound[] Music, SFX;
    public AudioSource musicSource, sfxSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            // Destroy duplicate managers in new scenes
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        PlayMusic("Game Music");
    }

    public void PlayMusic(string name)
    {
        Sound s = System.Array.Find(Music, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.volume = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = System.Array.Find(SFX, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        else
        {
            sfxSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
            sfxSource.PlayOneShot(s.clip);
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void StopSFX()
    {
        sfxSource.Stop();
    }

    public void MusicControl(float lower)
    {
        musicSource.volume = lower;
    }

    public void SFXControl(float lower)
    {
        sfxSource.volume = lower;
    }

    // 💡 Public method to change volume based on player settings
    /*public void SetVolume(float newVolume)
    {
        if (audioSource != null)
        {
            // The volume setting is applied to the one persistent AudioSource
            audioSource.volume = newVolume;
        }
    }*/
}