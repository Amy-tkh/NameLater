using UnityEngine;

public class UserManager : MonoBehaviour
{
    public static UserManager Instance { get; private set; } // The actual Singleton access

    [HideInInspector]
    public PlayerPersonality ActiveUser { get; private set; } // The reference to the data

    void Awake()
    {
        // STEP 1: Standard Singleton enforcement
        if (Instance == null)
        {
            Instance = this;
            // Ensure this object persists across scenes
            DontDestroyOnLoad(gameObject);

            // If the ActiveUser is null (which it is on first load), create it.
            // This is the line that was likely missing or not being executed.
            if (ActiveUser == null)
            {
                // **THIS IS THE LINE THAT FIXES THE NULL REFERENCE**
                ActiveUser = new PlayerPersonality();
                Debug.Log("[USER MANAGER] Successfully instantiated new PlayerPersonality.");
            }
        }
        else if (Instance != this)
        {
            // Destroy duplicate managers
            Destroy(gameObject);
        }
    }

    public void SetActiveUser(PlayerPersonality loadedData)
    {
        ActiveUser = loadedData;
        Debug.Log("User Manager: Successfully loaded new personality data.");
    }

    public PlayerPersonality GetActiveUser()
    {
        return ActiveUser;
    }
}