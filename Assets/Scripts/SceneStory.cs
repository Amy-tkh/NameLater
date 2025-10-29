using System.Collections.Generic;
using UnityEngine;

// This lets you right-click > Create > Story > Scene Story
[CreateAssetMenu(fileName = "NewSceneStory", menuName = "Story/Scene Story")]
public class SceneStory : ScriptableObject
{
    // This is just a list that will hold all the steps
    public List<EventStep> eventSteps;
}