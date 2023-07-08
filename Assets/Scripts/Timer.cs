using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float timeLeft;
    [SerializeField] private Text timerText;

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
            }
        }
    }

    public void updateTimer(float currentTime)
    {
        currentTime += 1;
        int currentMins = Mathf.FloorToInt(currentTime / 60);
        int currentSecs = Mathf.FloorToInt(currentTime % 60);
        timerText.text = string.Format("{0:00} {1:00}", currentMins, currentSecs);
    }
}
