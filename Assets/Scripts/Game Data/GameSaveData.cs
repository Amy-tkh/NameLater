using System;

[System.Serializable]
public class GameSaveData
{
    public PlayerPersonality playerPersonality;
    public int DialogueEventIndex;
    public int currentSceneIndex;
    public DateTime saveTimestamp;
    public float MusicVolume;
    public float SFXVolume;

    public GameSaveData()
    {
        playerPersonality = new PlayerPersonality();
        currentSceneIndex = 0; 
        DialogueEventIndex = 0;
        saveTimestamp = DateTime.Now;
    }

}