// DialogueManager.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("UI References")]
    private GameObject textBox;
    private TMP_Text charNameText;
    private RawImage nameBoxImage; 
    private RawImage dialogueBoxImage;
    private TMP_Text mainTextObject;
    private GameObject options;
    private GameObject nextButton;
    public GameObject DialogueUIPrefab;
    private GameObject currentUIInstance;
    
    void OnEnable()
    {
        // Subscribe to the sceneLoaded event
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Unsubscribe when the object is destroyed
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // This method will be called every time a new scene is loaded
    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        // Check the index of the NEWLY loaded scene
        if (scene.buildIndex != 0) // If it's not the Main Menu
        {
            Debug.Log($"[DialogueManager] Game scene (Index {scene.buildIndex}) loaded. Initializing UI.");
            InitializeUI();
        }
        else
        {
            Debug.Log($"[DialogueManager] Main Menu (Index 0) loaded. Skipping UI init.");
        }
    }

    /*void Awake()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex != 0)
        {
            InitializeUI();
        }
    }*/

    public void InitializeUI()
    {
        // Instantiate the UI into the current scene
        currentUIInstance = Instantiate(DialogueUIPrefab);
        if (currentUIInstance == null)
        {
            Debug.LogError("Failed to instantiate UI Prefab! Check DialogueUIPrefab reference.");
            return;
        }

        // Iterate through all children (active or inactive) to find "TextBox"
        Transform textBoxTransform = null;
        foreach (Transform child in currentUIInstance.transform)
        {
            if (child.name == "TextBox")
            {
                textBoxTransform = child;
                Debug.Log("Found 'TextBox' in UI Prefab.");
                break;
            }
        }

        if (textBoxTransform == null)
        {
            Debug.LogError("FATAL: Could not find 'TextBox' in UI Prefab. Check spelling.");
            return;
        }

        this.textBox = textBoxTransform.gameObject;
        this.options = textBoxTransform.Find("Options")?.gameObject;
        this.nextButton = textBoxTransform.Find("Button")?.gameObject;
        this.charNameText = textBoxTransform.Find("CharcaterNBox/CharName")?.GetComponent<TMP_Text>();
        this.nameBoxImage = textBoxTransform.Find("CharcaterNBox")?.GetComponent<RawImage>();
        this.dialogueBoxImage = textBoxTransform.GetComponent<RawImage>();
        this.mainTextObject = textBoxTransform.Find("SpeakingText")?.GetComponent<TMP_Text>();

        Button btn = this.nextButton.GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.RemoveAllListeners();
            
            // Find the BaseSceneEvents instance
            BaseSceneEvents sceneEvents = MainManager.Instance.SceneEvents;
            
            // Point the button to the new public function
            btn.onClick.AddListener(() => sceneEvents.NextButton());
        }
    }

    public void NextButtonFunction(ref List<System.Func<IEnumerator>> eventSequence, ref int eventPos)
    {
        Debug.LogWarning("this 2.1: " + eventPos);
        eventPos++;

        PlayerPrefs.SetInt("DialogueEventIndex", eventPos);

        if (eventPos < eventSequence.Count)
        {
            StartCoroutine(eventSequence[eventPos]());
            nextButton.SetActive(false); 
        }
        else
        {
            Debug.Log("End of scene events. eventPos is " + eventPos);
            nextButton.SetActive(false);
        }
    }

    public void SetSpeakingCharacter(CharacterData character)
    {
        if (character == null) return;

        textBox?.SetActive(true);
        if (charNameText != null)
            charNameText.text = character.characterName;
        if (nameBoxImage != null)
            nameBoxImage.color = character.nameBoxColor;
    }

    public void SetCharacterExpression(GameObject container, string expressionName)
    {
        foreach (Transform child in container.transform)
            child.gameObject.SetActive(false);
        Transform expr = container.transform.Find(expressionName);
        if (expr != null)
            expr.gameObject.SetActive(true);
        else
            Debug.LogError($"Expression '{expressionName}' not found in {container.name}");
    }

    public IEnumerator TypeOutText(string text, float charDelay = 0.03f)
    {
        textBox.SetActive(true);
        mainTextObject.gameObject.SetActive(true);
        if (mainTextObject == null) yield break; 

        mainTextObject.text = ""; 

        foreach (char c in text)
        {
            mainTextObject.text += c;
            yield return new WaitForSeconds(charDelay);
        }

        nextButton.SetActive(false);
        Debug.Log($"[DIALOGUE DEBUG] Typing out text: '{text}'");

        nextButton.SetActive(true);
    }
    
    public IEnumerator Speak(string characterName, string text)
    {

        CharacterData character = MainManager.Instance.CharOptions.GetCharacterByName(characterName);

        if (character == null)
        {

            Debug.LogError($"Character '{characterName}' not found!");

            yield break;

        }
        SetSpeakingCharacter(character);

        yield return StartCoroutine(TypeOutText(text));

    }

    public void AssignChoice(DialogueOption option, string buttonName, bool autoApplyToPersonality = true)
    {
        options.SetActive(true);
        nextButton.SetActive(false);
        mainTextObject.gameObject.SetActive(false);

        Button btn = options.transform.Find(buttonName).GetComponent<Button>();

        TMPro.TMP_Text text1 = btn.GetComponentInChildren<TMPro.TMP_Text>();
        if (text1 != null)
        {
            text1.text = option.text;
        }
        else
        {
            Debug.LogError("[DIALOGUE ERROR] Option1 button is missing a Text component in its children!");
        }
    }

    public void WhenOptionSelected(DialogueOption option, bool autoApplyToPersonality)
    {
        Debug.Log($"[DIALOGUE DEBUG] Player Selected: '{option.text}'");

        if (autoApplyToPersonality && option != null)
        {
            UserManager.Instance.ActiveUser.AddPoints(option);
            Debug.Log("[DIALOGUE DEBUG] Applied points to PlayerPersonality.");
        }

        BaseSceneEvents.optionChosen = "";


        options.SetActive(false);
        nextButton.SetActive(true);
        mainTextObject.gameObject.SetActive(true);
    }
}