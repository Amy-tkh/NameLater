using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Scene02Events : BaseSceneEvents
{
    public GameObject fadescreenIn;
    public GameObject fadeOut;
    public GameObject person2Container;
    public GameObject person1Container;

    public void Start()
    {
        PlayerPrefs.SetInt("currentSceneIndex", SceneManager.GetActiveScene().buildIndex);
        var events = new List<System.Func<IEnumerator>>
        {
            EventStarter,
            Event1,
            //Event2,
        };
        InitializeEvents(events);
    }

    protected override IEnumerator EventStarter()
    {
        yield return new WaitForSeconds(1);
        fadescreenIn.SetActive(false);
        Wait(1);
        Expression(person1Container, "Person1 default");
        Wait(1);
        Speak("yuki", "Oh god, I'm so tired...");
        Wait(1);
    }

    IEnumerator Event1()
    {
        yield return new WaitForSeconds(2);
        Speak("yuki", "What should I do...");
        yield return new WaitForSeconds(1);

        var opt1 = DialogueOption.Create("Do whatever you want", "protector", "thinker");
        var opt2 = DialogueOption.Create("Break the rules", "rebel", "masked one");
        yield return Pick(opt1, null, true);
        yield return Pick(opt2, null, true);

        yield return new WaitForSeconds(1);
        fadeOut.SetActive(true);
    }

    /*IEnumerator Event2()
    {
        Expression(person1Container, "Person1 default");
        Speak("yuki", "You're right... it doesn't.");
        yield return new WaitForSeconds(2);
        fadeOut.SetActive(true);
    }*/
}