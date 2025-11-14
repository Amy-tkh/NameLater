using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance { get; private set; }

    // Public fields to hold references to the manager scripts on this GameObject
    // These will be auto-populated in Awake or Start.
    public DialogueManager DialogueManager { get; private set; }
    public BaseSceneEvents SceneEvents { get; private set; }
    public CharacterOptions CharOptions { get; private set; }

    void Awake()
    {
        // 1. Singleton Enforcement (ensures only one copy exists)
        if (Instance == null)
        {
            Instance = this;
            // 2. Persistence
            DontDestroyOnLoad(this.gameObject);

            // 3. Get References to the child scripts on this same GameObject
            DialogueManager = GetComponent<DialogueManager>();
            if (DialogueManager == null) { Debug.LogError("DM NOT FOUND!"); }
            SceneEvents = GetComponent<BaseSceneEvents>();
            if (SceneEvents == null) { Debug.LogError("SE NOT FOUND!"); }
            CharOptions = GetComponent<CharacterOptions>();
            if (CharOptions == null) { Debug.LogError("CO NOT FOUND!"); }
        }
        else
        {
            // Destroy duplicate managers when loading a new scene
            Destroy(this.gameObject);
        }
    }
}
