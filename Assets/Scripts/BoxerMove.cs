using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxerMove : MonoBehaviour
{
    public bool canMove;
    private int targetIndex;

    public float speed;
    public int boxerNumber;

    public float startWaitTime;

    private float waitTime;

    [SerializeField] private SoundPlayer soundPlayer;
    [SerializeField] private SoundPlayable punch, hurt, whiff;
    [SerializeField] private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        canMove = true;

        waitTime = startWaitTime;
        targetIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove && GameController.begun)
        {
            Vector2 targetDirection = GameController.instance.fightPositions[boxerNumber].targetLocations[targetIndex].position - transform.position;

            float RotateSpeed = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, GameController.instance.fightPositions[boxerNumber].targetLocations[targetIndex].position, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, GameController.instance.fightPositions[boxerNumber].targetLocations[targetIndex].position) < .05f)
            {
                if (waitTime <= 0)
                {
                    targetIndex += 1;
                    if (targetIndex == GameController.instance.fightPositions[boxerNumber].targetLocations.Length)
                    {
                        targetIndex = 0;
                    }
                    StartCoroutine(ResetTime());
                }
                else
                {
                    waitTime -= Time.deltaTime;
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Boxer2")
        {
            print("boxers are close");
            GameController.instance.TakeAction();
        }
    }

    //fighter stands still, possibly blocks?
    public void Stand()
    {
        anim.SetTrigger("idle");
    }

    // public void Walk(string newDir)
    // {
    //     anim.SetTrigger("walk");
    //     soundPlayer.PlaySound(step);
    //     //Vector3 targetPosition = Vector3.zero;
    //     //Vector3 targetMove;//?
    //     int speed = 3;
    //     ChangeDir(newDir);
    //     // + transform.position;//or just have this saved?

    //     if (dir == "right")
    //     {
    //         transform.position = Vector3.MoveTowards(transform.position, transform.position + (speed * Vector3.right), speed * Time.deltaTime);
    //         //transform.position = new Vector3(transform.position.x + speed, transform.position.y, transform.position.z);
    //     }
    //     else if (dir == "left")
    //     {
    //         transform.position = Vector3.MoveTowards(transform.position, transform.position + (speed * Vector3.left), speed * Time.deltaTime);
    //     }
    //     else
    //     {
    //         //do nothing, I suppose
    //     }

    //     //transform.position = Vector3.MoveTowards(transform.position, transform.position + targetMove, speed * Time.deltaTime);

    // }

    // //fighter jumps into the air with force and lands, can jump in a chosen direction
    // public void Jump(string newDir)
    // {
    //     int speed = 1;
    //     anim.SetTrigger("jump");
    //     soundPlayer.PlaySound(jump);
    //     ChangeDir(newDir);

    //     if (!inAnimation)
    //     {
    //         m_Rigidbody.AddForce(new Vector2(0, 250));
    //         inAnimation = true;
    //     }
    //     else
    //     {
    //         if (m_Rigidbody.velocity.y == -250)
    //         {
    //             //    m_Rigidbody.velocity.x = 0;
    //             inAnimation = false;
    //         }
    //     }

    //     if (dir == "right")
    //     {
    //         transform.position = Vector3.MoveTowards(transform.position, transform.position + (speed * Vector3.right), 1);
    //         //transform.position = new Vector3(transform.position.x + speed, transform.position.y, transform.position.z);
    //     }
    //     else if (dir == "left")
    //     {
    //         transform.position = Vector3.MoveTowards(transform.position, transform.position + (speed * Vector3.left), 1);
    //     }


    // }
    // //character gets into a low evasive stance, optional and probably will not be used
    // public void Duck()
    // {
    //     anim.SetTrigger("duck");
    //     soundPlayer.PlaySound(duck);
    // }
    //character punches to try to hit the opponent
    //should be created so a message gets sent to fight parser determining if the attack is hit or block, or if it's a wiff
    public void Punch()
    {
        anim.SetTrigger("punch");
        soundPlayer.PlaySound(punch);
    }
    // public void Dash(string newDir)
    // {
    //     // ChangeDir(newDir);
    //     anim.SetTrigger("dash");
    //     soundPlayer.PlaySound(dash);
    // }
    //the fighter gets hit by an attack, cancels the previous action they were taking
    public void Hurt()
    {
        // add knockback, hit flash, etc idk
        anim.SetTrigger("hurt");
        soundPlayer.PlaySound(hurt);
        GetComponent<SimpleFlash>().Flash(2, 8, true);
    }

    public void Whiff()
    {
        // add knockback, hit flash, etc idk
        anim.SetTrigger("hurt");
        soundPlayer.PlaySound(whiff);
    }
    // public void KnockOut()
    // {
    //     anim.SetTrigger("knockout");
    //     soundPlayer.PlaySound(knockout);
    // }
    // public void GetUp()
    // {
    //     anim.SetTrigger("getup");
    //     soundPlayer.PlaySound(getup);
    // }

    IEnumerator ResetTime()
    {
        yield return new WaitForSeconds(0.5f);
        waitTime = startWaitTime;
    }
}
