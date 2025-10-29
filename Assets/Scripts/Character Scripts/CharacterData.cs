using UnityEngine;
using System; // Required for [Serializable]

// The [Serializable] attribute makes this class show up in the Inspector
[Serializable]
public class CharacterData
{
    // The name that will appear in the name box
    public string characterName;

    // The unique ID (optional, but useful for complex logic/loading)
    public string characterID;

    // The color for the name display box
    public Color nameBoxColor = Color.white;

    // The color for the main dialogue text box
    public Color dialogueBoxColor = Color.white;
}