using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterController : MonoBehaviour
{
    // Start is called before the first frame update
    //void Start()
    //{
    //    
    //}
    public string dir;//the direction the fighter is currently moving

    // Update is called once per frame
    //void Update()
    //{
    //    Walk(dir);
    //}

    void CheckWalkDir(string newDir){
        if(newDir != ""){
            dir = newDir;
        }
    }

    public void Stand(){}

    public void Walk(string newDir){
        //Vector3 targetPosition = Vector3.zero;
        //Vector3 targetMove;//?
        int speed = 10;
        CheckWalkDir(newDir);
        // + transform.position;//or just have this saved?

        if(dir == "right"){
           transform.position = Vector3.MoveTowards(transform.position, transform.position + (speed*Vector3.right), speed * Time.deltaTime);
        }
        else if(dir == "left"){
            transform.position = Vector3.MoveTowards(transform.position, transform.position + (speed*Vector3.left), speed * Time.deltaTime);
        }
        else{
            //do nothing, I suppose
        }

        //transform.position = Vector3.MoveTowards(transform.position, transform.position + targetMove, speed * Time.deltaTime);

    }

    public void Jump(string dir){}
    public void Duck(){}
    public void Punch(){}
    public void Dash(string dir){}


}
