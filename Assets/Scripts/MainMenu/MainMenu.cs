using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : BaseSceneEvents
{
    public GameObject fadeOut;
    public GameObject AudioContainer;

    public void StartNewGame()
    {
        PlayAudio("ButtonClick", AudioContainer);
        fadeOut.SetActive(true);
        StartCoroutine(LoadNewGameScene());
    }

    public void LoadGame()
    {
        PlayAudio("ButtonClick", AudioContainer);
        fadeOut.SetActive(true);
        StartCoroutine(LoadGameScene());
    }
    public void QuitGame()
    {
        PlayAudio("ButtonClick", AudioContainer);
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    private IEnumerator LoadGameScene()
    {
        yield return new WaitForSeconds(4);
        SaveManager.Instance.LoadGame();
    }

    private IEnumerator LoadNewGameScene()
    {
        yield return new WaitForSeconds(4);
        SaveManager.Instance.StartNewGame();
    }

}
