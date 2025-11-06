using System.IO; 
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    private string saveFilePath;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        
        // save file name
        string saveFileName = "saveSlot1.json";

        
        // build file path
        saveFilePath = Path.Combine(Application.persistentDataPath, saveFileName);

        Debug.Log("Save file will be located at: " + saveFilePath);
    }

    public void SaveGame()
    {
        // Create a new GameSaveData object to hold all the data
        GameSaveData saveData = new GameSaveData();

        // Populate it with current game data
        saveData.playerPersonality = UserManager.Instance.GetActiveUser();
        saveData.currentSceneIndex = PlayerPrefs.GetInt("currentSceneIndex", 0);
        saveData.DialogueEventIndex = PlayerPrefs.GetInt("DialogueEventIndex", 0);
        saveData.saveTimestamp = System.DateTime.Now;

        // Convert the data to a JSON string
        string json = JsonUtility.ToJson(saveData, true); // 'true' formats it nicely

        // Write the JSON string to the file at your new path
        File.WriteAllText(saveFilePath, json);

        Debug.Log("Game saved!");
    }
    
    public void StartNewGame()
    {
        Debug.Log("Starting new game...");

        // 1. DELETE THE SAVE FILE
        // Check if the file exists before trying to delete it
        if (File.Exists(saveFilePath))
        {
            try
            {
                File.Delete(saveFilePath);
                Debug.Log("Old save file deleted.");
            }
            catch (System.Exception e)
            {
                Debug.LogError("Could not delete save file: " + e.Message);
            }
        }

        // 2. CLEAR PLAYERPREFS
        // This resets your saved scene and event index
        PlayerPrefs.DeleteKey("DialogueEventIndex");
        PlayerPrefs.DeleteKey("currentSceneIndex");
        // (Add any other keys you save, like "DialogueLineIndex")
        PlayerPrefs.DeleteKey("DialogueLineIndex"); 
        Debug.Log("PlayerPrefs cleared.");

        // 3. RESET LIVE DATA (VERY IMPORTANT!)
        // Resets the data in your persistent UserManager, in case a
        // game was previously loaded.
        if (UserManager.Instance != null)
        {
            // We use the "setter" method you created
            UserManager.Instance.SetActiveUser(new PlayerPersonality());
            Debug.Log("Live user data has been reset.");
        }
        
        // 4. LOAD THE FIRST SCENE
        // Assumes your first game scene is at build index 1
        // Change '1' if your first scene is different.
        SceneManager.LoadScene(1); 
    }

    public void LoadGame()
    {
        // Check if the save file actually exists
        if (File.Exists(saveFilePath))
        {
            // Read all the text from the file
            string json = File.ReadAllText(saveFilePath);

            // Convert the JSON back into your C# object
            GameSaveData loadedData = JsonUtility.FromJson<GameSaveData>(json);

            // Now, apply this loaded data to your game managers
            UserManager.Instance.SetActiveUser(loadedData.playerPersonality);
            PlayerPrefs.SetInt("currentSceneIndex", loadedData.currentSceneIndex);
            PlayerPrefs.SetInt("DialogueEventIndex", loadedData.DialogueEventIndex);

            // Finally, load the correct scene
            SceneManager.LoadScene(loadedData.currentSceneIndex);
            
            Debug.Log("Game loaded!");
        }
        else
        {
            Debug.LogWarning("No save file found! Starting new game.");
            SceneManager.LoadScene(1); // Load the first scene if no save exists
        }
    }
}