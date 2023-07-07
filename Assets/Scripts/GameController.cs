using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int currentLevel = 0;
    public static GameController instance;

    public int fighterOneScore;
    public int fighterTwoScore;
    public int scoreToWin;

    public int suspicion;
    public int susToLose;

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

    //awards points to a figther based on the player's command
    void AwardScore(){
        //if f1 button
        //fighterOneScore += 1;
        //if f2 button
        //fighterTwoScore +=2;

        //if suspicous
        //add suspicion

        //CheckVictory()
    }

    //check if the player's current action is suspicious, add apropriate suspicion if so
    void CheckSusAction(){
        //make if statements for fighters
        
        //if player 1 awared and player 1 hit, add 0%
        //if player 1 blocked, add 20%
        //if player 1 wiffed, add 40%
        //if player 1 didn't punch, add 80%

        //same for player 2
    }

    //decide if the player won or lost
    void CheckVictory(){
        if(suspicion >= susToLose){
            //lose
        }
        else if(fighterOneScore >= scoreToWin){
            //win
        }
        else if(fighterTwoScore >= scoreToWin){
            //lose
        }
    }
}
