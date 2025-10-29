using UnityEngine;
using UnityEngine.UI;

public class ScenePlayer : MonoBehaviour
{
    // 1. DATA TO PLAY
    // You drag your "Scene01Story.asset" file here
    public SceneStory currentSceneStory; 

    // 2. REFERENCES
    // Link your managers just like you do now
    public DialogueManager dialogueManager;
    public Button nextButton;
    public GameObject person1Container; // etc.
    public GameObject audioContainer;
    
    // 3. STATE
    private int eventPos = 0; // The current "bookmark"

    void Start()
    {
        // Get the bookmark from memory
        eventPos = PlayerPrefs.GetInt("DialogueEventIndex", 0);
        
        // Check if bookmark is bad
        if (eventPos < 0 || eventPos >= currentSceneStory.eventSteps.Count)
        {
            eventPos = 0;
            PlayerPrefs.SetInt("DialogueEventIndex", 0);
        }
        
        // Link the "Next" button to our 'Next' function
        // You can also do this in the Inspector
        nextButton.onClick.AddListener(OnNextButtonClick); 
        
        // Play the first (or saved) event
        PlayEvent(eventPos);
    }
    
    void OnNextButtonClick()
    {
        // 1. Move bookmark to the next event
        eventPos++;
        
        // 2. Check if we're at the end
        if (eventPos < currentSceneStory.eventSteps.Count)
        {
            // 3. Play the next event
            PlayEvent(eventPos);
        }
        else
        {
            // 4. End of the scene
            Debug.Log("Scene finished!");
            nextButton.gameObject.SetActive(false);
            // Put your 'LoadScene("Scene02")' logic here
        }
    }

    // This is the "master" function that reads the data and makes it happen
    void PlayEvent(int index)
    {
        // 1. Get the data for the current step
        EventStep step = currentSceneStory.eventSteps[index];

        // 2. Save our new bookmark
        PlayerPrefs.SetInt("DialogueEventIndex", index);
        
        // 3. Use the data!
        // This solves your save/load state problem.
        // The data *is* the state!
        if (!string.IsNullOrEmpty(step.expressionToSet))
        {
            // You'd need to make your 'Expression' function public
            dialogueManager.SetCharacterExpression(person1Container, step.expressionToSet);
        }
        
        if (!string.IsNullOrEmpty(step.audioName))
        {
            dialogueManager.SetCharacterAudio(person1Container, step.audioName);
        }
        
        // This is the only logic that's still here
        dialogueManager.Speak(step.speakerName, step.dialogueText);
    }
}