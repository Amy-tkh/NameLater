// DialogueManager.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    /*public static DialogueManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // This is what keeps the object alive!
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            // Destroy any duplicates if a new scene tries to create one
            Destroy(this.gameObject);
        }
    }*/

    [Header("UI References")]
    [SerializeField] protected CharacterOptions charOptions;
    public GameObject textBox;
    public TMP_Text charNameText;
    public RawImage nameBoxImage; 
    public RawImage dialogueBoxImage;
    public TMP_Text mainTextObject;
    public GameObject options;
    public GameObject nextButton;
    private DialogueOption selectedOption = null;

    public IEnumerator WaitCoroutine(int seconds)
    {
        yield return new WaitForSeconds(seconds);
    }


    public void NextButtonFunction(ref List<System.Func<IEnumerator>> eventSequence, ref int eventPos)
    {
        Debug.LogWarning("this 2.1: " + eventPos);
        eventPos++;

        // 2. Save the new bookmark
        PlayerPrefs.SetInt("DialogueEventIndex", eventPos);

        // 3. Check if this new bookmark is valid
        if (eventPos < eventSequence.Count)
        {
            // 4. Play the event at the NEW position
            StartCoroutine(eventSequence[eventPos]());
            nextButton.SetActive(false); // Hide button until event is done
        }
        else
        {
            // We are at the end of the scene
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

    public void SetCharacterAudio(GameObject container, string audioName)
    {
        foreach (Transform child in container.transform)
            child.gameObject.SetActive(false);

        Transform audio = container.transform.Find(audioName);
        if (audio != null)
        {
            audio.gameObject.SetActive(true);
            audio.GetComponent<AudioSource>()?.Play();
        }
        else
            Debug.LogError($"Audio '{audioName}' not found in {container.name}");
    }

    public IEnumerator TypeOutText(string text, float charDelay = 0.03f)
    {
        textBox.SetActive(true);
        mainTextObject.gameObject.SetActive(true);
        if (mainTextObject == null) yield break; // Safety check

        mainTextObject.text = ""; // Clear the text

        // Loop through each character and add it to the visible text
        foreach (char c in text)
        {
            mainTextObject.text += c;
            yield return new WaitForSeconds(charDelay);
        }

        nextButton.SetActive(false);
        Debug.Log($"[DIALOGUE DEBUG] Typing out text: '{text}'");

        nextButton.SetActive(true);
    }
    
    public void Speak(string characterName, string text)
    {

        CharacterData character = charOptions?.GetCharacterByName(characterName);

        if (character == null)
        {

            Debug.LogError($"Character '{characterName}' not found!");

            return;

        }
        SetSpeakingCharacter(character);
        StartCoroutine(TypeOutText(text));

    }

    public IEnumerator AssignChoice(DialogueOption option, string buttonName, bool autoApplyToPersonality = true)
    {
        // Assign options to UI buttons
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

        // Wait until player picks
        yield return new WaitUntil(() => selectedOption != null);

        // Debug 3: Selection confirmation
        Debug.Log($"[DIALOGUE DEBUG] Player Selected: '{selectedOption.text}'");

        // Apply the choice
        if (autoApplyToPersonality && selectedOption != null)
        {
            UserManager.Instance.ActiveUser.AddPoints(selectedOption);
        }

        // Clean up
        options.SetActive(false);
        nextButton.SetActive(true);
        mainTextObject.gameObject.SetActive(true);

    }

    void OnOptionSelected(DialogueOption option)
    {
        selectedOption = option;
    }

    // Getter to retrieve the result
    public DialogueOption GetLastSelectedOption()
    {
        return selectedOption;
    }
}