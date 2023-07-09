using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Countdown : MonoBehaviour
{
    public TextMeshProUGUI text;
    [SerializeField] private Animator anim;
    [SerializeField] private SoundPlayer soundPlayer;
    [SerializeField] private SoundClip countdown;

    public void StartCountdown()
    {
        anim.SetTrigger("start");
    }
    public void PlaySound()
    {
        soundPlayer.PlaySound(countdown);
    }

    public void SetText(string val)
    {
        text.text = val;
    }
}
