using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    public UnityEngine.UI.Button startButton;
    public static MainMenuManager main;
    public static bool inMainMenu;
    private bool quit = false;
    public SoundClip click;
    public SoundPlayer soundPlayer;

    // Start is called before the first frame update
    void Start()
    {
        main = this;
        inMainMenu = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        EventSystem.current.SetSelectedGameObject(startButton.gameObject);
    }

    public void Update()
    {
        if((Input.GetAxisRaw("Vertical") != 0) && EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(startButton.gameObject);
        }
    }

    public void StartGame()
    {
        if (!ChangeScene.changingScene && Crossfade.over && !quit)
        {
            soundPlayer.PlaySound(click);
            AudioManager.instance.FadeOutCurrent();
            ChangeScene.LoadScene("Arena");
            //CanvasManager.ShowHUD();
        }
    }

    public void Quit()
    {
        if (!ChangeScene.changingScene && Crossfade.over && !quit)
        {
            quit = true;
            soundPlayer.PlaySound(click);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            StartCoroutine(QuitRoutine());
        }
    }

    public void ShowCredits()
    {
        soundPlayer.PlaySound(click);
        // show credits panel and hide the other stuff
    }

    public void ShowInstructions()
    {
        soundPlayer.PlaySound(click);
        // show instructions panel and hide the other stuff
    }

    public void ReturnToMainMenu()
    {
        soundPlayer.PlaySound(click);
        // hide credits/instructions panel and show the other stuff
    }

    IEnumerator QuitRoutine()
    {
        Crossfade.FadeStart();
        AudioManager.instance.FadeOutCurrent();
        yield return new WaitForSeconds(1);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OnDestroy()
    {
        if (!quit)
        {
            PauseScreen.canPause = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

}