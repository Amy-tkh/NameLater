using System.Collections.Generic;
using UnityEngine;

public class CharacterOptions : MonoBehaviour
{
    [SerializeField] private List<CharacterData> allCharacters = new List<CharacterData>();
    public List<CharacterData> AllCharacters => allCharacters; // Simplified getter
    void Awake()
    {
        var yuki = new CharacterData
        {
            characterName = "yuki",
            nameBoxColor = new Color(240f / 255f, 136f / 255f, 136f / 255f),
            dialogueBoxColor = Color.lightPink
        };
        allCharacters.Add(yuki);

        var haruka = new CharacterData
        {
            characterName = "haruka",
            nameBoxColor = Color.lightBlue,
            dialogueBoxColor = Color.cyan
        };
        allCharacters.Add(haruka);
    }

    public CharacterData GetCharacterByName(string name)
    {
        return allCharacters.Find(c =>
            string.Equals(c.characterName, name, System.StringComparison.OrdinalIgnoreCase)
        );
    }
}