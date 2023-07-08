using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public int currentLevel = 0;
    public static GameController instance;

    public int fighterOneScore;
    public Text fighterOneText;
    public int fighterTwoScore;
    public Text fighterTwoText;
    public int scoreToWin;

    public int suspicion;
    public int susToLose;

    public Image susMeter;

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
            susMeter = GameObject.Find("Canvas/SuspicionBar/SuspicionBarFill").GetComponent<Image>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1")){
            AwardScore(0);
        }
        else if(Input.GetButtonDown("Fire2")){
            AwardScore(1);
        }
        UpdateSusMeter();
    }

    //awards points to a figther based on the player's command
    void AwardScore(int fighter){
        //if f1 button
        if(fighter == 0){
            fighterOneScore += 1;
            fighterOneText.text = fighterOneScore + "";
        }
        //if f2 button
        else if(fighter == 1){
            fighterTwoScore += 1;
            fighterTwoText.text = fighterTwoScore + "";
        }
        //if suspicous
        //add suspicion
        CheckSusAction(fighter);
    }

    //check if the player's current action is suspicious, add apropriate suspicion if so
    void CheckSusAction(int fighter){
        //make if statements for fighters
        
        //if player 1 awared and player 1 hit, add 0%
        //if player 1 blocked, add 20%
        //if player 1 wiffed, add 40%
        //if player 1 didn't punch, add 80%

        //same for player 2

        //update sus meter
        UpdateSusMeter();

        CheckVictory();
    }

    //add suspicion if no ation taken when aproprate
    //activate when no score after good hit, somehow have way to check(perhaps put a timer to here when good hit takes place)
    void CheckSusInaction(){
        //if [winning?] fighter
        //suspicion += 10%
        //if [losing?] fighter
        //suspicion += 20%

        UpdateSusMeter();

        CheckVictory();
    }

    //updates the suspicion meter//TODO TEST TO SEE IF DONE CORRECTLY
    void UpdateSusMeter(){
        susMeter.fillAmount = suspicion / (float)susToLose;
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
