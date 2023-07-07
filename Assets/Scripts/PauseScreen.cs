using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseScreen : MonoBehaviour
{
    public static bool paused = false;
    public static bool canPause = true;
    public static bool quit = true;

    CanvasGroup pauseMenuGroup;

    public CanvasGroup pauseContainerGroup;

    public UnityEngine.UI.Button resumeButton;

    public CanvasGroup quitPrompt;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenuGroup = GetComponent<CanvasGroup>();
        //Debug.Log(Camera.main.pixelWidth + "  " + Camera.main.pixelHeight);
    }

    // Update is called once per frame
    void Update()
    {

        if ((Input.GetAxisRaw("Vertical") != 0) && EventSystem.current.currentSelectedGameObject == null && paused)
        {
            EventSystem.current.SetSelectedGameObject(resumeButton.gameObject);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (ChangeScene.changingScene || !Crossfade.over)
            {
                return;
            }
            
            if (!paused)
            {
                Pause();
            }
            else
            {
                unPause();
            }
        }
    }
    public void Pause()
    {
        if (!canPause)
            return;
        AudioSource[] sources = FindObjectsOfType<AudioSource>();
        paused = true;
        Time.timeScale = 0;
        foreach (AudioSource source in sources)
        {
            source.Pause();
        }
        pauseMenuGroup.alpha = 1;
        pauseMenuGroup.blocksRaycasts = true;
        pauseMenuGroup.interactable = true;
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(resumeButton.gameObject);

        pauseContainerGroup.alpha = 1;
        pauseContainerGroup.blocksRaycasts = true;
        pauseContainerGroup.interactable = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void unPause()
    {
        AudioSource[] sources = FindObjectsOfType<AudioSource>();
        paused = false;
        Time.timeScale = 1;
        foreach (AudioSource source in sources)
        {
            source.UnPause();
        }
        // TODO super sloppy but re-pauses music + dialog speakers
        if (AudioManager.instance.paused)
            AudioManager.instance.PauseCurrent();
        
        pauseMenuGroup.alpha = 0;
        pauseMenuGroup.blocksRaycasts = false;
        pauseMenuGroup.interactable = false;

        pauseContainerGroup.alpha = 0;
        pauseContainerGroup.blocksRaycasts = false;
        pauseContainerGroup.interactable = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void PromptQuit()
    {
        quitPrompt.alpha = 1;
        quitPrompt.blocksRaycasts = true;
        quitPrompt.interactable = true;

        pauseMenuGroup.alpha = 0;
        pauseMenuGroup.blocksRaycasts = false;
        pauseMenuGroup.interactable = false;
    }

    public void CancelPrompt()
    {
        closePrompt();

        pauseMenuGroup.alpha = 1;
        pauseMenuGroup.blocksRaycasts = true;
        pauseMenuGroup.interactable = true;
    }

    public void closePrompt()
    {
        quitPrompt.alpha = 0;
        quitPrompt.blocksRaycasts = false;
        quitPrompt.interactable = false;
    }

    public void QuitToTitle()
    {
        StartCoroutine(BackToMenu());
    }

    public IEnumerator BackToMenu() {
        quit = true;
        closePrompt();
        unPause();
        ChangeScene.changingScene = true;
        // CanvasManager.InstantHideHUD();
        AudioManager.instance.FadeOutCurrent();
        Crossfade.current.StartFade();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
        ChangeScene.clearCollisions?.Invoke();
        ChangeScene.clearInteractables?.Invoke();
        Crossfade.current.StopFade();
        quit = false;
    }
}