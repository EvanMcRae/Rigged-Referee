using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxerUnoMove : MonoBehaviour
{
    public bool canMove;

    public Transform[] targetLocations;
    private int targetIndex;

    public float speed;

    public float startWaitTime;

    private float waitTime;
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
        Vector2 targetDirection = targetLocations[targetIndex].position - transform.position;

        float RotateSpeed = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetLocations[targetIndex].position, speed * Time.deltaTime);

        //if (Vector3.Magnitude(targetDirection) > .1)
        //{
        //    targetDirection.y = 0;
        //    transform.rotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        //}

        if (Vector2.Distance(transform.position, targetLocations[targetIndex].position) < .05f)
        {
            if (waitTime <= 0)
            {
                targetIndex += 1;
                if (targetIndex == targetLocations.Length)
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

    IEnumerator ResetTime()
    {
        yield return new WaitForSeconds(0.5f);
        waitTime = startWaitTime;
    }
}
