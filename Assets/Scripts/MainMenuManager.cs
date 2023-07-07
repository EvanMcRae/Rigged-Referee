using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    public UnityEngine.UI.Button startButton;

    public static bool inMainMenu;

    // Start is called before the first frame update
    void Start()
    {
        inMainMenu = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        //EventSystem.current.SetSelectedGameObject(newGameButton.gameObject);
    }

    public void Update()
    {
        if((Input.GetAxisRaw("Vertical") != 0) && EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(startButton.gameObject);
        }
    }

    public void Quit()
    {
        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public string GetMostRecentSave()
    {
        string[] files = Directory.GetFiles(Application.persistentDataPath + @"\SaveData");

        string mostRecentFile = "";
        System.DateTime mostRecentDate = System.DateTime.Now;
        foreach(string file in files)
        {
            if(File.GetLastWriteTime(file) < System.DateTime.Now)
            {
                mostRecentDate = File.GetLastWriteTime(file);
                mostRecentFile = file;
            }
        }
        Debug.Log("most recent save: " + mostRecentFile + " accessed on " + mostRecentDate.ToString());

        return mostRecentFile;
    }
    public void OnDestroy()
    {
        PauseScreen.canPause = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

}

[System.Serializable]
public class MenuBackgrounds
{
    public GameObject Forest;
    public GameObject Fields;
    public GameObject Caves;
    public GameObject Town;
    public GameObject Desert;
}