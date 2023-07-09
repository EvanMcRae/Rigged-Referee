using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    public UnityEngine.UI.Button startButton, creditsButton, returnCredits, instructionsButton, returnInstructions;
    public static MainMenuManager main;
    public static bool inMainMenu;
    private bool quit = false, inCredits = false, inInstructions = false;
    public SoundClip click;
    public SoundPlayer soundPlayer;
    public GameObject creditsPanel;
    public GameObject instructionsPanel;

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
            if (inCredits)
            {
                EventSystem.current.SetSelectedGameObject(returnCredits.gameObject);
            }
            else if (inInstructions)
            {
                EventSystem.current.SetSelectedGameObject(returnInstructions.gameObject);
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(startButton.gameObject);
            }
        }
    }

    public void StartGame()
    {
        if (!ChangeScene.changingScene && !quit && !inCredits && !inInstructions)
        {
            soundPlayer.PlaySound(click);
            AudioManager.instance.FadeOutCurrent();
            ChangeScene.LoadScene("Arena");
            //CanvasManager.ShowHUD();
        }
    }

    public void Quit()
    {
        if (!ChangeScene.changingScene && !quit && !inCredits && !inInstructions)
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
        if (!ChangeScene.changingScene && !quit && !inCredits && !inInstructions)
        {
            soundPlayer.PlaySound(click);
            creditsPanel.SetActive(true);
            inCredits = true;
        }
    }

    public void ShowInstructions()
    {
        if (!ChangeScene.changingScene && !quit && !inCredits && !inInstructions)
        {
            soundPlayer.PlaySound(click);
            instructionsPanel.SetActive(true);
            inInstructions = true;
        }
    }

    public void ReturnToMainMenu()
    {
        soundPlayer.PlaySound(click);

        if (inCredits)
        {
            creditsPanel.SetActive(false);
            EventSystem.current.SetSelectedGameObject(creditsButton.gameObject);
            inCredits = false;
        }

        if (inInstructions)
        {
            instructionsPanel.SetActive(false);
            EventSystem.current.SetSelectedGameObject(instructionsButton.gameObject);
            inInstructions = false;
        }
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