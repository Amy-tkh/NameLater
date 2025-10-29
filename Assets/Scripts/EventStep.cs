using UnityEngine;

[CreateAssetMenu(fileName = "EventStep", menuName = "Scriptable Objects/EventStep")]
public class EventStep : ScriptableObject
{
    // These are the fields you'll fill in the Inspector
    public string speakerName;
    
    [TextArea(3, 10)] // Makes the text box bigger in the Inspector
    public string dialogueText;  
    public string expressionToSet; // e.g., "Person1 sad" 
    public string audioName;
    public bool fadeOut;
    public bool fadeIn;
    
}