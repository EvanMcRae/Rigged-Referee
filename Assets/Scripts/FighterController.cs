using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterController : MonoBehaviour
{
    public string dir;//the direction the fighter is currently moving
    //public bool test;
    private bool jumping;
    private Rigidbody2D m_Rigidbody;
    [SerializeField] private SoundPlayer soundPlayer;
    [SerializeField] private Animator anim;

        // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    void ChangeDir(string newDir){
        if(newDir != ""){
            dir = newDir;
        }
        transform.localScale = new Vector3(dir == "left" ? -1 : 1, 1, 1);
    }

    public void Stand(){
        anim.SetTrigger("idle");
    }

    public void Walk(string newDir){
        anim.SetTrigger("walk");
        //Vector3 targetPosition = Vector3.zero;
        //Vector3 targetMove;//?
        int speed = 1;
        ChangeDir(newDir);
        // + transform.position;//or just have this saved?

        if(dir == "right"){
           transform.position = Vector3.MoveTowards(transform.position, transform.position + (speed*Vector3.right), 1);
           //transform.position = new Vector3(transform.position.x + speed, transform.position.y, transform.position.z);
        }
        else if(dir == "left"){
            transform.position = Vector3.MoveTowards(transform.position, transform.position + (speed*Vector3.left), 1);
        }
        else{
            //do nothing, I suppose
        }

        //transform.position = Vector3.MoveTowards(transform.position, transform.position + targetMove, speed * Time.deltaTime);

    }

    public void Jump(string newDir){
        int speed = 1;
        anim.SetTrigger("jump");
        ChangeDir(newDir);

        if(!jumping){
            m_Rigidbody.AddForce(new Vector2(0, 250));
            jumping = true;
        }
        else{
            if(m_Rigidbody.velocity.y == -250){
            //    m_Rigidbody.velocity.x = 0;
                jumping = false;
            }
        }

        if(dir == "right"){
           transform.position = Vector3.MoveTowards(transform.position, transform.position + (speed*Vector3.right), 1);
           //transform.position = new Vector3(transform.position.x + speed, transform.position.y, transform.position.z);
        }
        else if(dir == "left"){
            transform.position = Vector3.MoveTowards(transform.position, transform.position + (speed*Vector3.left), 1);
        }
            

    }
    public void Duck(){
        anim.SetTrigger("duck");
    }
    public void Punch(){
        anim.SetTrigger("punch");
    }
    public void Dash(string newDir){
        ChangeDir(newDir);
        anim.SetTrigger("dash");
    }

    public void Hurt() {
        // add knockback, hit flash, etc idk
        anim.SetTrigger("hurt");
    }
}
