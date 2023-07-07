using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FightParser : MonoBehaviour
{
    [SerializeField] private FighterController f1, f2;
    [SerializeField] private string file;
    private string script;

    // Start is called before the first frame update
    void Start()
    {
        script = File.ReadAllText(Path.Combine(Application.streamingAssetsPath + @"\FightScript\", file));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
