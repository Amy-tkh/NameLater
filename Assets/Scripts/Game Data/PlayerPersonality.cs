using UnityEngine;

[System.Serializable]
public class DialogueOption
{
    public string text;

    // Two personality traits this option affects
    public string majorTrait;
    public string minorTrait;

    // Optional: if you want direct point control (for future flexibility)
    public int protectorPoints;
    public int maskedOnePoints;
    public int thinkerPoints;
    public int rebelPoints;

    // Helper method to create an option (optional)
    public static DialogueOption Create(string text, string major, string minor)
    {
        var option = new DialogueOption
        {
            text = text,
            majorTrait = major.ToLower(),
            minorTrait = minor.ToLower()
        };

        // Auto-assign points based on traits
        option.protectorPoints = (option.majorTrait == "protector" || option.minorTrait == "protector") ? 1 : 0;
        option.maskedOnePoints = (option.majorTrait == "masked one" || option.minorTrait == "masked one") ? 1 : 0;
        option.thinkerPoints = (option.majorTrait == "thinker" || option.minorTrait == "thinker") ? 1 : 0;
        option.rebelPoints = (option.majorTrait == "rebel" || option.minorTrait == "rebel") ? 1 : 0;

        return option;
    }
}

[System.Serializable]
public class PlayerPersonality // NO MonoBehaviour, NO Singleton logic
{
    // These are the properties that define ONE user's stats
    public int ProtectorPoints { get; set; }
    public int ThinkerPoints { get; set; }
    public int RebelPoints { get; set; }
    public int MaskedOnePoints { get; set; }
    public void AddPoints(DialogueOption option)
    {
        ProtectorPoints += option.protectorPoints;
        ThinkerPoints += option.thinkerPoints;
        RebelPoints += option.rebelPoints;
        MaskedOnePoints += option.maskedOnePoints;
        // ... logic ...
    }
}