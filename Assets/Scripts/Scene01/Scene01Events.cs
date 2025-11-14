using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene01Events : BaseSceneEvents
{
    // Only scene-specific fields
    public GameObject fadescreenIn;
    public GameObject fadeOut;
    public GameObject person2Container;
    public GameObject person1Container;

    public void Start()
    {
        // Define scene-specific events
        PlayerPrefs.SetInt("currentSceneIndex", SceneManager.GetActiveScene().buildIndex);
        var events = new List<System.Func<IEnumerator>>
        {
            Event1,
            Event2,
            Event3,
            Event4,
        };
        InitializeEvents(events); // Handled by base class
    }

    IEnumerator Event1()
    {
        yield return new WaitForSeconds(1);
        fadescreenIn.SetActive(false);
        yield return new WaitForSeconds(1);
        Expression(person2Container, "Person2 normal");
        Expression(person1Container, "Person1 sad");
        yield return new WaitForSeconds(2);
        Speak("yuki", "What are you doing here?");
        yield return new WaitForSeconds(2);
        SaveManager.Instance.SaveGame();
    }

    IEnumerator Event2()
    {
        Expression(person2Container, "Person2 tired");
        PlayAudio("GirlSigh");
        Speak("haruka", "Why would it matter to you?");
        yield return new WaitForSeconds(2);
        SaveManager.Instance.SaveGame();
    }

    IEnumerator Event3()
    {
        Expression(person1Container, "Person1 default");
        Speak("yuki", "You're right... it doesn't.");
        yield return new WaitForSeconds(2);
        fadeOut.SetActive(true);
        yield return new WaitForSeconds(4);
        PlayerPrefs.SetInt("DialogueEventIndex", 0);
        SceneManager.LoadScene("Scene02");
        yield return new WaitForSeconds(1);
        SaveManager.Instance.SaveGame();
    }

    IEnumerator Event4()
    {
        yield return new WaitForSeconds(1);
        fadeOut.SetActive(true);
    }


    protected override void SetSceneState(int eventIndex)
    {
        // This state is true for ANY event after the start
        fadescreenIn.SetActive(false);

        // If we are loading Event 1 (or later)
        if (eventIndex >= 1)
        {
            // Apply the state from Event 1
            Expression(person2Container, "Person2 normal");
            Expression(person1Container, "Person1 sad");
        }

        // If we are loading Event 2 (or later)
        if (eventIndex >= 2)
        {
            // Apply the state from Event 2
            Expression(person2Container, "Person2 tired");
        }

        // If we are loading Event 3 (or later)
        if (eventIndex >= 3)
        {
            // Apply the state from Event 3
            Expression(person1Container, "Person1 default");
        }
    }
}