using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BaseSceneEvents : MonoBehaviour
{
    [Header("Shared References")]
    DialogueManager dm; 
    CharacterOptions co;
    protected List<System.Func<IEnumerator>> eventSequence = new();
    protected int eventPos = 0;
    public static string optionChosen = "";

    void Awake()
    {
        try
        {
        // This is the line that is crashing
        dm = MainManager.Instance.DialogueManager; 

        // This log will only appear if the line above *succeeds*
        if (dm == null)
        {
            Debug.LogError("[BaseSceneEvents] 'dm' is NULL *after* assignment.");
        }
        else
        {
            Debug.Log("[BaseSceneEvents] 'dm' was successfully assigned!");
        }
        }
        catch (System.Exception e)
        {
            // This will CATCH the silent crash and print it
            Debug.LogError($"[BaseSceneEvents] CRASH IN AWAKE: {e.Message}");
        }
    }

    protected void InitializeEvents(List<System.Func<IEnumerator>> events)
    {
        eventSequence = events;

        eventPos = PlayerPrefs.GetInt("DialogueEventIndex", 0);

        if (eventPos < 0 || eventPos >= eventSequence.Count)
        {
            Debug.LogWarning("Invalid eventPos loaded. Resetting to 0.");
            eventPos = 0;
            PlayerPrefs.SetInt("DialogueEventIndex", 0); // Save the reset
        }

        // 1. Restore the visual state based on the event we are about to run
        SetSceneState(eventPos);

        StartCoroutine(eventSequence[eventPos]());
    }

    protected virtual IEnumerator EventStarter()
    {
        yield return null;
    }

    protected virtual void SetSceneState(int eventIndex)
    {
        // Default implementation does nothing
    }

    public void NextButton()
    {
        dm.NextButtonFunction(ref eventSequence, ref eventPos);
    }

    // Helper methods
    public void Speak(string characterName, string text)
    {
        dm.StartCoroutine(dm.Speak(characterName, text));
    }

    public void PlayAudio(string audioName)
    {
        AudioManager.Instance.PlaySFX(audioName);
    }

    public void Expression(GameObject container, string expressionName)
    {
        dm?.SetCharacterExpression(container, expressionName);
    }

    public IEnumerator Pick(DialogueOption opt1, DialogueOption opt2, bool autoApplyToPersonality = true)
    {
        Debug.Log("[PICK DEBUG] Pick coroutine started."); // NEW
        dm.AssignChoice(opt1, "Option1", autoApplyToPersonality);
        dm.AssignChoice(opt2, "Option2", autoApplyToPersonality);

        Debug.Log("[PICK DEBUG] Waiting for optionChosen to be set..."); // NEW

        yield return new WaitUntil(() => BaseSceneEvents.optionChosen != "");
        Debug.Log($"[PICK DEBUG] Coroutine woke up. optionChosen is: {BaseSceneEvents.optionChosen}"); // Shows what was read

        DialogueOption selectedOption = null;

        if (optionChosen == "opt1")
        {
            selectedOption = opt1;
            Debug.Log("[PICK DEBUG] Matched opt1. selectedOption is NOT null.");
        }
        else if (optionChosen == "opt2")
        {
            selectedOption = opt2;
            Debug.Log("[PICK DEBUG] Matched opt2. selectedOption is NOT null.");
        }
        else
        {
            // If you see the "Coroutine woke up" log but NOT the "Matched" log, 
            // it means the string comparison failed, and this log should appear:
            Debug.LogError($"[PICK ERROR] optionChosen value '{optionChosen}' did not match 'opt1' or 'opt2'."); 
        }

        if (selectedOption != null)
        {
            dm.WhenOptionSelected(selectedOption, autoApplyToPersonality);
            // You should now see the WhenOptionSelected log!
        }

        BaseSceneEvents.optionChosen = "";
    }

    public void GiveSelectedOption(string buttonName)
    {
        Debug.Log($"[GSO DEBUG] Clicked: {buttonName}"); // NEW
        if (buttonName == "Option1")
        {
            BaseSceneEvents.optionChosen = "opt1";
            Debug.Log(BaseSceneEvents.optionChosen);
        }
        else if (buttonName == "Option2")
        {
            BaseSceneEvents.optionChosen = "opt2";
            Debug.Log(BaseSceneEvents.optionChosen);
        }
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

    public void OpenMainMenu()
    {
        SaveManager.Instance.SaveGame();
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        SaveManager.Instance.SaveGame();
        Debug.Log("Quitting game...");
        Application.Quit();
    }

}