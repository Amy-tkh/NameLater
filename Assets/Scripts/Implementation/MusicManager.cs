using UnityEngine;



public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    private AudioSource audioSource; // Reference to the AudioSource component

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            
            // Get the AudioSource component
            audioSource = GetComponent<AudioSource>(); 
            
            // 💡 ONLY START THE MUSIC HERE, ONCE
            if (audioSource != null && !audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            // Destroy duplicate managers in new scenes
            Destroy(this.gameObject);
        }
    }

    // 💡 Public method to change volume based on player settings
    public void SetVolume(float newVolume)
    {
        if (audioSource != null)
        {
            // The volume setting is applied to the one persistent AudioSource
            audioSource.volume = newVolume;
        }
    }
}