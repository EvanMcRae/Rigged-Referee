using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float timeLeft;
    [SerializeField] private Text timerText;
    [SerializeField] private SoundClip endCountdown;
    private bool fiveSecs;

    int finalCheck = 0;

    // Update is called once per frame
    void Update()
    {
        if (GameController.begun)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                updateTimer(timeLeft);
            }
            else
            {
                timeLeft = 0;
                if(finalCheck == 0)
                {
                    print("timer hit ZERO");
                    GameController.instance.CheckVictoryTimedOut();
                    finalCheck++;
                }
            }
        }
    }

    public void updateTimer(float currentTime)
    {
        currentTime += 1;
        int currentMins = Mathf.FloorToInt(currentTime / 60);
        int currentSecs = Mathf.FloorToInt(currentTime % 60);
        if (currentSecs == 5 && currentMins == 0 && !fiveSecs)
        {
            GameController.instance.soundPlayer.PlaySound(endCountdown);
            fiveSecs = true;
        }
        timerText.text = string.Format("{0:00} {1:00}", currentMins, currentSecs);
    }
}
