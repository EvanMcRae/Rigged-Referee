using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Countdown : MonoBehaviour
{
    public TextMeshProUGUI text;
    [SerializeField] private Animator anim;

    public void StartCountdown()
    {
        anim.SetTrigger("start");
    }

    public void SetText(string val)
    {
        text.text = val;
    }
}
