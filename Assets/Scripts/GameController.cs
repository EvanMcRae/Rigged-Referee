using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public static bool begun = false;
    public int stage = 0;
    [SerializeField] private Text stageLabel;
    [SerializeField] private Timer timer;
    [SerializeField] private Countdown countdown;
    [SerializeField] private GameObject[] fighterOnePrefabs, fighterTwoPrefabs;
    public FightPositions[] fightPositions;
    private GameObject fighterOne, fighterTwo;

    public int fighterOneScore;
    public Text fighterOneText;
    public int fighterTwoScore;
    public Text fighterTwoText;
    public int scoreToWin;

    public int suspicion;
    public int susToLose;

    [SerializeField] private Image susMeter;

    public SoundPlayer soundPlayer;
    [SerializeField] private SoundClip winSound, loseSound;
    public UnityEngine.UI.Button WinButton, LoseButton;

    //are the fighters taking an action
    private bool inAction;
    //which action is being taken
    private int choice;

    public GameObject WinMenu;

    public GameObject LoseMenu;

    // Start is called before the first frame update
    void Start()
    {
        //susMeter = GameObject.Find("Canvas/SuspicionBar/SuspicionBarFill").GetComponent<Image>();
        //timer = GameObject.Find("Canvas/Timer").GetComponent<Timer>();
        //stageLabel = GameObject.Find("Canvas/StageLabel").GetComponent<Text>();
        instance = this;
        inAction = false;
        nextStage();
    }

    // Update is called once per frame
    void Update()
    {
        if (begun) 
        {
            if (Input.GetButtonDown("Fire1"))
            {
                print("awarded fighter one a point");
                AwardScore(0);
            }
            else if (Input.GetButtonDown("Fire2"))
            {
                print("awarded fighter two a point");
                AwardScore(1);
            }
            UpdateSusMeter();
        }
    }

    public void TakeAction(){
        if(!inAction){
            inAction = true;
            StartCoroutine(DecideAction());
        }
    }

    IEnumerator DecideAction(){
        //two second wait in between punches
        fighterOne.GetComponent<BoxerMove>().canMove = false;
        fighterTwo.GetComponent<BoxerMove>().canMove = false;
        yield return new WaitForSeconds(2f);

        choice = Random.Range(1, 5);
        
        //fighter 1 lands hit
        if(choice == 1){
            fighterOne.GetComponent<BoxerMove>().Punch();
            fighterTwo.GetComponent<BoxerMove>().Hurt();
        }
        //fighter 1 gets blocked
        else if(choice == 2){
            fighterOne.GetComponent<BoxerMove>().Punch();
            //make player two block//maybe just have them stand?
        }
        //fighter 2 lands hit
        else if(choice == 3){
            fighterOne.GetComponent<BoxerMove>().Hurt();
            fighterTwo.GetComponent<BoxerMove>().Punch();
        }
        //fighter 2 gets blocked
        else if(choice == 4){
            //make f1 block
            fighterTwo.GetComponent<BoxerMove>().Punch();
        }
        //idle, but probably won't need to be used
        else{
            //idle
            fighterOne.GetComponent<BoxerMove>().Stand();
            fighterTwo.GetComponent<BoxerMove>().Stand();
        }

        //each punch gives two seconds to react
        yield return new WaitForSeconds(2f);
        //set both idle, mostly to make animations tranitions smoother
        fighterOne.GetComponent<BoxerMove>().Stand();
        fighterTwo.GetComponent<BoxerMove>().Stand();
        yield return new WaitForSeconds(.067f);

        //if fighter wasn't awarded points they desterve, raise suspicion
        CheckSusInaction();

        //resetchoice
        choice = 0;
        inAction = false;
        fighterOne.GetComponent<BoxerMove>().canMove = true;
        fighterTwo.GetComponent<BoxerMove>().canMove = true;
    }

    //awards points to a figther based on the player's command
    void AwardScore(int fighter){
        //if f1 button
        if(fighter == 0){
            fighterOneScore += 1;
            fighterOneText.text = string.Format("{0:000}", fighterOneScore);

            //TODO also rig up ref holding flag here
        }
        //if f2 button
        else if(fighter == 1){
            fighterTwoScore += 1;
            fighterTwoText.text = string.Format("{0:000}", fighterTwoScore);

            //TODO here as well
        }
        //if suspicous, add suspicion
        CheckSusAction(fighter);

        //ensure that single action can only be called out once
        choice = 0;
    }

    //check if the player's current action is suspicious, add apropriate suspicion if so //might want to change values
    void CheckSusAction(int fighter){
        //if no one punched, add 50% suspicion
        if(choice == 0){
            suspicion += 50;
        }
        //if scoring f1 on a good hit
        else if(choice == 1 && fighter == 0){
            suspicion += 0;
        }
        //if scoring f2 on a bad hit
        else if(choice == 2 && fighter == 0){
            suspicion += 10;
        }
        //if scoreing f2 on a good hit
        else if(choice == 3 && fighter == 1){
            suspicion += 0;
        }
        //if scoring fighter 2 on a bad hit
        else if(choice == 3 && fighter == 1){
            suspicion += 0;
        }
        //if making a false score entirely
        else{
            suspicion += 50;
        }

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

        //if fighter 1 hit
        if(choice == 1){

        }
        //if fighter 2 hit and no points were awarded
        if(choice == 3){
            suspicion += 20;
        }

        UpdateSusMeter();

        CheckVictory();
    }

    //updates the suspicion meter
    void UpdateSusMeter(){
        susMeter.fillAmount = suspicion / (float)susToLose;
    }

    //decide if the player won or lost
    public void CheckVictory(){
        print("checking for victory");
        if(suspicion >= susToLose){
            LoseGame();
        }
        else if(fighterOneScore >= scoreToWin){
            if (stage == 3)
                WinGame();
            else
                WinStage();
        }
        else if(fighterTwoScore >= scoreToWin){
            LoseGame();
        }
    }

    public void CheckVictoryTimedOut()
    {
        print("TIMED OUT, and checking for victory");
        if (suspicion >= susToLose)
        {
            LoseGame();
        }
        else if (fighterOneScore >= scoreToWin)
        {
            if (stage == 3)
                WinGame();
            else
                WinStage();
        }
        else if (fighterTwoScore >= scoreToWin)
        {
            LoseGame();
        }
        else if(fighterTwoScore > fighterOneScore)
        {
            LoseGame();
        }
        else if(fighterTwoScore < fighterOneScore)
        {
            if (stage == 3)
                WinGame();
            else
                WinStage();
        }
        else if(fighterTwoScore == fighterOneScore)
        {
            LoseGame();
        }
    }

    void LoseGame()
    {
        print("Lose Game");
        LoseMenu.GetComponent<WinLoseScreen>().showLosePanel();
        EventSystem.current.SetSelectedGameObject(LoseButton.gameObject);
        begun = false;
        soundPlayer.PlaySound(loseSound);
        AudioManager.instance.FadeOutCurrent(0.5f);
        // show lose dialog with return to menu button
    }

    void WinStage()
    {
        print("Win Stage");
        WinMenu.GetComponent<WinLoseScreen>().showWinPanel();
        EventSystem.current.SetSelectedGameObject(WinButton.gameObject);
        begun = false;
        soundPlayer.PlaySound(winSound);
        // show win dialog with advance to next stage button
    }

    void WinGame()
    {
        print("Win Game");
        WinMenu.GetComponent<WinLoseScreen>().showWinPanel();
        EventSystem.current.SetSelectedGameObject(WinButton.gameObject);
        begun = false;
        soundPlayer.PlaySound(winSound);
        // show win dialog with return to menu button
    }

    public void nextStage()
    {
        print("next stage!");
        begun = false;
        if (stage < 3)
            StartCoroutine(PrepareStage());
    }

    IEnumerator PrepareStage()
    {
        // reset from previous stage, if applicable
        if (stage > 0)
        {
            Crossfade.FadeStart();
            yield return new WaitForSeconds(1.0f);
            
            GameObject.Destroy(fighterOne);
            GameObject.Destroy(fighterTwo);
            fighterOneText.text = string.Format("{0:000}", (fighterOneScore = 0));
            fighterTwoText.text = string.Format("{0:000}", (fighterTwoScore = 0));
            susMeter.fillAmount = (suspicion = 0);

            Crossfade.FadeEnd();
        }

        fighterOne = GameObject.Instantiate(fighterOnePrefabs[stage], new Vector3(-5.0f, -3.2f, 0f), Quaternion.identity);
        fighterTwo = GameObject.Instantiate(fighterTwoPrefabs[stage], new Vector3(5.0f, -3.2f, 0f), Quaternion.identity);

        // TODO have different times per stage?
        timer.timeLeft = 180f;
        timer.updateTimer(timer.timeLeft - 1);
        
        stageLabel.text = (stage+1) + "";

        countdown.StartCountdown();

        yield return new WaitForSeconds(3.0f);
        stage++;
        begun = true;
    }
}
