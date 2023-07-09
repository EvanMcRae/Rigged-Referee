using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed = 5f;
    private float movementX;
    [SerializeField] private Animator anim;
    [SerializeField] private SoundPlayer soundPlayer;
    [SerializeField] private SoundClip whistle, flag, count;

    // Start is called before the first frame update
    void Start()
    {
        Idle();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
            {
                FlagLeft();
            }
        else if (Input.GetButtonDown("Fire2"))
            {
                FlagRight();
            }
    }

    void MovePlayer(){
        movementX = Input.GetAxisRaw("Horizontal");
        transform.position += new Vector3(movementX, 0f, 0f) * moveSpeed * Time.deltaTime;
    }

    public void Idle()
    {
        anim.SetTrigger("idle");
    }

    public void FlagLeft()
    {
        anim.SetTrigger("flagleft");
    }

    public void FlagRight()
    {
        anim.SetTrigger("flagright");
    }
}
