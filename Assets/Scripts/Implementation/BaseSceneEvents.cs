using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseSceneEvents : MonoBehaviour
{
    [Header("Shared References")]
    [SerializeField] protected DialogueManager dialogueManager;
    [SerializeField] protected CharacterOptions charOptions;
    protected List<System.Func<IEnumerator>> eventSequence = new();
    protected int eventPos = 0;

    // Call this in Start() of each scene to set up events
    protected void InitializeEvents(List<System.Func<IEnumerator>> events)
    {
        eventSequence = events;

        // Get the bookmark
        eventPos = PlayerPrefs.GetInt("DialogueEventIndex", 0);

        // Check if the bookmark is bad
        if (eventPos < 0 || eventPos >= eventSequence.Count)
        {
            Debug.LogWarning("Invalid eventPos loaded. Resetting to 0.");
            eventPos = 0;
            PlayerPrefs.SetInt("DialogueEventIndex", 0); // Save the reset
        }

        // Start the correct event
        StartCoroutine(eventSequence[eventPos]());
    }

    protected virtual void SetSceneState(int eventIndex)
    {
        // Default implementation does nothing
    }

    // Shared NextButton logic — works for ALL scenes
    public void NextButton()
    {
        dialogueManager.NextButtonFunction(ref eventSequence, ref eventPos);
    }

    // Helper methods
    public void Speak(string characterName, string text)
    {

        CharacterData character = charOptions?.GetCharacterByName(characterName);

        if (character == null)
        {

            Debug.LogError($"Character '{characterName}' not found!");

            return;

        }
        dialogueManager?.SetSpeakingCharacter(character);
        StartCoroutine(dialogueManager.TypeOutText(text));

    }

    public void PlayAudio(string audioName, GameObject characterAudioContainer)
    {
        dialogueManager?.SetCharacterAudio(characterAudioContainer, audioName);
    }

    public void Expression(GameObject container, string expressionName)
    {
        dialogueManager?.SetCharacterExpression(container, expressionName);
    }

    // Virtual so scenes can override if needed
    protected virtual IEnumerator EventStarter()
    {
        yield break; // Default: do nothing
    }

    protected void Wait(int seconds)
    {
        StartCoroutine(dialogueManager.WaitCoroutine(seconds));
    }

    public void Pick(DialogueOption opt1, DialogueOption opt2, bool autoApplyToPersonality = true)
    {
        StartCoroutine(dialogueManager.AssignChoice(opt1, "Option1", autoApplyToPersonality));
        StartCoroutine(dialogueManager.AssignChoice(opt2, "Option2", autoApplyToPersonality));
    }

    public void selectOption(DialogueOption option)
    {
        dialogueManager.OnOptionSelected(option);
    }

    public void OpenSetting(GameObject settingBG)
    {
        if (settingBG.activeSelf == false)
        {
            settingBG.SetActive(true);
        }
        else
        {
            settingBG.SetActive(false);
        }
    }

}