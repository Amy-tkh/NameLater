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
            sfxSource.PlayOneShot(s.clip);
        }
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