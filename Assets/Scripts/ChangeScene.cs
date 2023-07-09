using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class ChangeScene : MonoBehaviour
{
    public string scene;
    public string spawn;
    public static bool changingScene = false;
    public static Action clearCollisions, clearInteractables;

    IEnumerator LoadNextScene()
    {
        changingScene = true;
        Crossfade.FadeStart();
        yield return new WaitForSeconds(1f);
        DisableMenuMusic();
        EventSystem eventSystem = GameObject.FindObjectOfType<EventSystem>();
        GameObject.Destroy(eventSystem?.gameObject);
        SceneHelper.LoadScene(scene);
        clearCollisions?.Invoke();
        clearInteractables?.Invoke();
        Crossfade.changeScene?.Invoke();
        changingScene = false;
    }

    public static void LoadScene(string scene)
    {
        MainMenuManager.main.StartCoroutine(LoadSceneEnum(scene));
    }

    static IEnumerator LoadSceneEnum(string scene)
    {
        changingScene = true;
        Crossfade.current.StartFade();
        yield return new WaitForSeconds(1f);
        DisableMenuMusic();
        EventSystem eventSystem = GameObject.FindObjectOfType<EventSystem>();
        if (eventSystem != null)
        {
            GameObject.Destroy(eventSystem.gameObject);
        }
        SceneHelper.LoadScene(scene);
        clearCollisions?.Invoke();
        clearInteractables?.Invoke();
        //Crossfade.changeScene?.Invoke();
        Crossfade.current.StopFade();
        changingScene = false;
    }

    public static void DisableMenuMusic()
    {
        if (MainMenuManager.inMainMenu)
        {
            AudioManager.instance.Stop();
            MainMenuManager.inMainMenu = false;
        }
    }
}