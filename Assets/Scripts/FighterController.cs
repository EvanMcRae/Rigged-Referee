using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterController : MonoBehaviour
{
    public string dir;//the direction the fighter is currently moving
    //public bool test;
    private bool inAnimation;
    private Rigidbody2D m_Rigidbody;
    [SerializeField] private SoundPlayer soundPlayer;
    [SerializeField] private SoundClip step, jump, punch, hurt, knockout, getup, dash, duck;
    [SerializeField] private Animator anim;

        // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    //fixed update always runs 60 times per second, independent of the frame rate
    //private void FixedUpdate() {
        //if(!inAnimation && GameController.begun){
        //    StartCoroutine(TakeAction());
        //}
    //}

    //basic possible example of what AI could look like
    private IEnumerator TakeAction(){
        int choice = Random.Range(0, 2);

        if(choice == 0){
            inAnimation = true;
            for(int i = 0; i < 6; i++){
                Walk("left");
                yield return new WaitForSeconds(.016f);//set to .016f seconds or aprox 1 frame
            }
            inAnimation = false;
        }
        else if(choice == 1){
            inAnimation = true;
            for(int i = 0; i < 6; i++){
                Walk("right");
                yield return new WaitForSeconds(.016f);//set to .016f seconds or aprox 1 frame
            }
            inAnimation = false;
        }
        else if(choice == 2){

        }
        else{

        }
        
    }

    void ChangeDir(string newDir){
        if(newDir != ""){
            dir = newDir;
        }
        // transform.localScale = new Vector3(dir == "left" ? -1 : 1, 1, 1);
    }

    //Most of code below currently assumes that a script is being given to tell what to do next, change as needed

    //fighter stands still, possibly blocks?
    public void Stand(){
        anim.SetTrigger("idle");
    }

    public void Walk(string newDir){
        anim.SetTrigger("walk");
        soundPlayer.PlaySound(step);
        //Vector3 targetPosition = Vector3.zero;
        //Vector3 targetMove;//?
        int speed = 3;
        ChangeDir(newDir);
        // + transform.position;//or just have this saved?

        if(dir == "right"){
           transform.position = Vector3.MoveTowards(transform.position, transform.position + (speed*Vector3.right), speed*Time.deltaTime);
           //transform.position = new Vector3(transform.position.x + speed, transform.position.y, transform.position.z);
        }
        else if(dir == "left"){
            transform.position = Vector3.MoveTowards(transform.position, transform.position + (speed*Vector3.left), speed*Time.deltaTime);
        }
        else{
            //do nothing, I suppose
        }

        //transform.position = Vector3.MoveTowards(transform.position, transform.position + targetMove, speed * Time.deltaTime);

    }

    //fighter jumps into the air with force and lands, can jump in a chosen direction
    public void Jump(string newDir){
        int speed = 1;
        anim.SetTrigger("jump");
        soundPlayer.PlaySound(jump);
        ChangeDir(newDir);

        if(!inAnimation){
            m_Rigidbody.AddForce(new Vector2(0, 250));
            inAnimation = true;
        }
        else{
            if(m_Rigidbody.velocity.y == -250){
            //    m_Rigidbody.velocity.x = 0;
                inAnimation = false;
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
    //character gets into a low evasive stance, optional and probably will not be used
    public void Duck(){
        anim.SetTrigger("duck");
        soundPlayer.PlaySound(duck);
    }
    //character punches to try to hit the opponent
    //should be created so a message gets sent to fight parser determining if the attack is hit or block, or if it's a wiff
    public void Punch(){
        anim.SetTrigger("punch");
        soundPlayer.PlaySound(punch);
    }
    public void Dash(string newDir){
        ChangeDir(newDir);
        anim.SetTrigger("dash");
        soundPlayer.PlaySound(dash);
    }
    //the fighter gets hit by an attack, cancels the previous action they were taking
    public void Hurt() {
        // add knockback, hit flash, etc idk
        anim.SetTrigger("hurt");
        soundPlayer.PlaySound(hurt);
        GetComponent<SimpleFlash>().Flash(2, 8, true);
    }
    public void KnockOut()
    {
        anim.SetTrigger("knockout");
        soundPlayer.PlaySound(knockout);
    }
    public void GetUp()
    {
        anim.SetTrigger("getup");
        soundPlayer.PlaySound(getup);
    }
}
