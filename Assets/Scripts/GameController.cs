using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int currentLevel = 0;
    public static GameController instance;

    // Start is called before the first frame update
    void Start()
    {
        // Singleton behavior
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        if (instance != this)
        {
            Destroy(gameObject);
        }
        else 
        {
            GetComponent<FightParser>().StartFight(currentLevel);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
