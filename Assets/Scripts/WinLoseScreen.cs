using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class WinLoseScreen : MonoBehaviour
{
    CanvasGroup winLoseMenuGroup;
    public UnityEngine.UI.Button mainMenuButton;
    public UnityEngine.UI.Button nextStageButton;
    public SoundPlayer soundPlayer;
    public SoundClip click;

    public GameObject pauseRef;
    // Start is called before the first frame update
    void Start()
    {
        winLoseMenuGroup = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void showWinPanel()
    {
        winLoseMenuGroup.alpha = 1;
        winLoseMenuGroup.blocksRaycasts = true;
        winLoseMenuGroup.interactable = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void showLosePanel()
    {
        winLoseMenuGroup.alpha = 1;
        winLoseMenuGroup.blocksRaycasts = true;
        winLoseMenuGroup.interactable = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void clickNext()
    {
        GameController.instance.nextStage();

        winLoseMenuGroup.alpha = 0;
        winLoseMenuGroup.blocksRaycasts = false;
        winLoseMenuGroup.interactable = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void clickMainMenu()
    {
        pauseRef.GetComponent<PauseScreen>().QuitToTitle();
    }

}
