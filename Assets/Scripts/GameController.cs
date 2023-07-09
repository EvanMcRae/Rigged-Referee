using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public static bool begun = false;
    public int stage = 0;
    [SerializeField] private Text stageLabel;
    [SerializeField] private Timer timer;
    [SerializeField] private GameObject[] fighterOnePrefabs, fighterTwoPrefabs;
    private GameObject fighterOne, fighterTwo;

    public int fighterOneScore;
    public Text fighterOneText;
    public int fighterTwoScore;
    public Text fighterTwoText;
    public int scoreToWin;

    public int suspicion;
    public int susToLose;

    [SerializeField] private Image susMeter;

    [SerializeField] private SoundPlayer soundPlayer;
    [SerializeField] private SoundClip winSound, loseSound, matchStart, matchEnd;

    //are the fighters taking an action
    private bool inAction;
    //which action is being taken
    private int choice;

    // Start is called before the first frame update
    void Start()
    {
        //susMeter = GameObject.Find("Canvas/SuspicionBar/SuspicionBarFill").GetComponent<Image>();
        //timer = GameObject.Find("Canvas/Timer").GetComponent<Timer>();
        //stageLabel = GameObject.Find("Canvas/StageLabel").GetComponent<Text>();

        inAction = false;
        nextStage();
    }

    // Update is called once per frame
    void Update()
    {
        TakeAction();

        if(Input.GetButtonDown("Fire1")){
            AwardScore(0);
        }
        else if(Input.GetButtonDown("Fire2")){
            AwardScore(1);
        }
        UpdateSusMeter();

        // TODO REMOVE - DEBUG!!
        if (Input.GetKeyDown(KeyCode.Y))
        {
            nextStage();
        }
    }

    void TakeAction(){
        if(!inAction){
            inAction = true;
            StartCoroutine(DecideAction());
        }
    }

    IEnumerator DecideAction(){
        //two second wait in between punches
        yield return new WaitForSeconds(2f);

        choice = Random.Range(1, 5);
        
        //fighter 1 lands hit
        if(choice == 1){
            
            fighterOne.GetComponent<FighterController>().Punch();
            fighterTwo.GetComponent<FighterController>().Hurt();
        }
        //fighter 1 gets blocked
        else if(choice == 2){
            fighterOne.GetComponent<FighterController>().Punch();
            //make player two block//maybe just have them stand?
        }
        //fighter 2 lands hit
        else if(choice == 3){

            fighterOne.GetComponent<FighterController>().Hurt();
            fighterTwo.GetComponent<FighterController>().Punch();
        }
        //fighter 2 gets blocked
        else if(choice == 4){
            //make f1 block
            fighterTwo.GetComponent<FighterController>().Punch();
        }
        //idle, but probably won't need to be used
        else{
            //idle
            fighterOne.GetComponent<FighterController>().Stand();
            fighterTwo.GetComponent<FighterController>().Stand();
        }

        //each punch gives two seconds to react
        yield return new WaitForSeconds(2f);
        //set both idle, mostly to make animations tranitions smoother
        fighterOne.GetComponent<FighterController>().Stand();
        fighterTwo.GetComponent<FighterController>().Stand();
        yield return new WaitForSeconds(.067f);

        //if fighter wasn't awarded points they desterve, raise suspicion
        CheckSusInaction();

        //resetchoice
        choice = 0;
        inAction = false;
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
    void CheckVictory(){
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

    void LoseGame()
    {
        begun = false;
        soundPlayer.PlaySound(loseSound);
        // show lose dialog with return to menu button
    }

    void WinStage()
    {
        begun = false;
        soundPlayer.PlaySound(matchEnd);
        // show win dialog with advance to next stage button
    }

    void WinGame()
    {
        begun = false;
        soundPlayer.PlaySound(winSound);
        // show win dialog with return to menu button
    }

    public void nextStage()
    {
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
        timer.timeLeft = 60f;
        timer.updateTimer(timer.timeLeft - 1);
        
        stageLabel.text = (stage+1) + "";
        soundPlayer.PlaySound(matchStart);

        // show animated 3-2-1 countdown text in the middle of the screen

        yield return new WaitForSeconds(3.0f);
        stage++;
        begun = true;
    }
}
