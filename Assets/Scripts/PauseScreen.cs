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
    public UnityEngine.UI.Button resumeButton;
    public SoundPlayer soundPlayer;
    public SoundClip click;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenuGroup = GetComponent<CanvasGroup>();
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
        foreach (AudioSource source in soundPlayer.sources)
        {
            source.UnPause();
        }
        pauseMenuGroup.alpha = 1;
        pauseMenuGroup.blocksRaycasts = true;
        pauseMenuGroup.interactable = true;
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(resumeButton.gameObject);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void unPause()
    {
        soundPlayer.PlaySound(click, 0.5f);
        AudioSource[] sources = FindObjectsOfType<AudioSource>();
        paused = false;
        Time.timeScale = 1;
        foreach (AudioSource source in sources)
        {
            source.UnPause();
        }
        if (AudioManager.instance.paused)
            AudioManager.instance.PauseCurrent();
        
        pauseMenuGroup.alpha = 0;
        pauseMenuGroup.blocksRaycasts = false;
        pauseMenuGroup.interactable = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void QuitToTitle()
    {
        soundPlayer.PlaySound(click, 0.5f);
        StartCoroutine(BackToMenu());
    }

    public IEnumerator BackToMenu() {
        quit = true;
        unPause();
        ChangeScene.changingScene = true;
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