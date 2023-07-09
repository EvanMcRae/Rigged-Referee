using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchEffect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Remove", 0.5f);
    }

    void Remove()
    {
        GameObject.Destroy(gameObject);
    }
}
