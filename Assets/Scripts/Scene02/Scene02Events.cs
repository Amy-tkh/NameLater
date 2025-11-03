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
        };
        InitializeEvents(events);
    }

    protected override IEnumerator EventStarter()
    {
        yield return new WaitForSeconds(2);
        fadescreenIn.SetActive(false);
        yield return new WaitForSeconds(1);
        Expression(person1Container, "Person1 default");
        yield return new WaitForSeconds(1);
        Speak("yuki", "Oh god, I'm so tired...");
        yield return new WaitForSeconds(1);
        SaveManager.Instance.SaveGame();
    }

    IEnumerator Event1()
    {
        yield return new WaitForSeconds(2);
        Speak("yuki", "What should I do...");
        yield return new WaitForSeconds(4);

        var opt1 = DialogueOption.Create("Do whatever you want", "protector", "thinker");
        var opt2 = DialogueOption.Create("Break the rules", "rebel", "masked one");
        yield return Pick(opt1, opt2, true);


        yield return new WaitForSeconds(1);
        SaveManager.Instance.SaveGame();
        fadeOut.SetActive(true);
    }
}