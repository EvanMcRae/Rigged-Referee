using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterController : MonoBehaviour
{
    public string dir;//the direction the fighter is currently moving
    //public bool test;
    private bool jumping;
    private Rigidbody2D m_Rigidbody;

        // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
    //    
    }

    void CheckWalkDir(string newDir){
        if(newDir != ""){
            dir = newDir;
        }
    }

    public void Stand(){}

    public void Walk(string newDir){
        //Vector3 targetPosition = Vector3.zero;
        //Vector3 targetMove;//?
        int speed = 1;
        CheckWalkDir(newDir);
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
        CheckWalkDir(newDir);

        if(!jumping){
            m_Rigidbody.AddForce(new Vector2(0, 250));
            jumping = true;
        }
        else{
            if(m_Rigidbody.velocity.y == -250){
                jumping = false;
            }
        }
    }
    public void Duck(){}
    public void Punch(){}
    public void Dash(string dir){}


}
