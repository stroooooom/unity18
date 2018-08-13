using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController> {

    public void LoadMenu()
    {
        StartCoroutine(LoadMenuScene());
    }

    public void SinglePlayerScene()
    {
        StartCoroutine(LoadSinglePlayerScene());
    }

    public void MultiplayerScene()
    {
        StartCoroutine(LoadMultiplayerScene());
    }

    private IEnumerator LoadMenuScene()
    {
        var loadingScene = SceneManager.LoadSceneAsync("StartMenu");
        while (!loadingScene.isDone)
        {
            //Debug.Log("Loading scene: " + loadingScene.progress);
            yield return null;
        }
    }

    private IEnumerator LoadSinglePlayerScene()
    {
        var loadingScene = SceneManager.LoadSceneAsync("TankTutorial");
        while (!loadingScene.isDone)
        {
            //Debug.Log("Loading scene: " + loadingScene.progress);
            yield return null;
        }
    }

    private IEnumerator LoadMultiplayerScene()
    {
        var loadingScene = SceneManager.LoadSceneAsync("TankTutorialMultiplayer");
        while (!loadingScene.isDone)
        {
            //Debug.Log("Loading scene: " + loadingScene.progress);
            yield return null;
        }
    }
}
