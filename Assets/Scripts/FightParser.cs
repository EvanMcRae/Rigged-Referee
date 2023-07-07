using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FightParser : MonoBehaviour
{
    [SerializeField] private FighterController[] fighters;
    [SerializeField] private string file;
    private string[] script;
    [Range(0f, 3f)][SerializeField] private float delay = 1f;
    private float lastAction = 0f;
    private int index = 0;
    private bool running = true;

    // Start is called before the first frame update
    void Start()
    {
        script = File.ReadAllLines(Path.Combine(Application.streamingAssetsPath + @"\FightScript\", file));
    }

    // Update is called once per frame
    void Update()
    {
        if (lastAction + delay > Time.time && running)
        {
            // Filter out comments + whitespace
            while (script[index].Trim().StartsWith("#")
                || String.IsNullOrWhiteSpace(script[index]))
                index++;
            
            // In case we scrolled too far
            if (index >= script.Length) 
            {
                running = false;
                return;
            }

            // Gather commands into parsable lists
            string[] actions = script[index].ToLower().Split(",");
            string[][] fightAction = { actions[0].Split(" ") , actions[1].Split(" ") };

            // Iterate over both fighters
            for (int i = 0; i < 2; i++)
            {
                if (fightAction[i].Length > 2)
                {
                    Debug.LogError("Too many inputs for Fighter " + (i+1) + "!");
                }

                switch (fightAction[i][0])
                {
                    case "walk":
                        if (fightAction[i].Length > 1)
                        {
                            fighters[i].Walk(fightAction[i][1]);
                        }
                        else
                        {
                            fighters[i].Walk("");
                        }
                        break;

                    case "jump":
                        if (fightAction[i].Length > 1)
                        {
                            fighters[i].Jump(fightAction[i][1]);
                        }
                        else
                        {
                            fighters[i].Jump("");
                        }
                        break;

                    case "duck":
                        fighters[i].Duck();
                        break;

                    case "punch":
                        fighters[i].Punch();
                        break;

                    case "dash":
                        if (fightAction[i].Length > 1)
                        {
                            fighters[i].Dash(fightAction[i][1]);
                        }
                        else
                        {
                            fighters[i].Dash("");
                        }
                        break;

                    case "stand":
                    default:
                        // do anything here?
                        break;
                }
            }

            // Increment values for next run
            index++;
            if (index >= script.Length)
            {
                running = false;
            }
            lastAction = Time.time;
        }
    }
}
