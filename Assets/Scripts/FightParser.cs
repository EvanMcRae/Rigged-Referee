using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FightParser : MonoBehaviour
{
    [SerializeField] private FighterController[] fighters;
    [SerializeField] private int levelNumber;
    private string[] script;
    [Range(0f, 3f)][SerializeField] private float delay = 1f;
    private float lastAction = 0f;
    private int index = 0;
    private bool running = true;

    // Start is called before the first frame update
    void Start()
    {
        string file = "L" + levelNumber + ".txt";
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
            string[][] fighterActions = { actions[0].Split(" ") , actions[1].Split(" ") };

            // Iterate over both fighters
            for (int i = 0; i < 2; i++)
            {
                if (fighterActions[i].Length < 1)
                {
                    Debug.LogError("Not enough inputs for Fighter " + (i + 1) + "! " + actions[i]);
                    break;
                }
                else if (fighterActions[i].Length > 2)
                {
                    Debug.LogError("Too many inputs for Fighter " + (i+1) + "! " + actions[i]);
                    break;
                }

                switch (fighterActions[i][0])
                {
                    case "walk":
                        if (fighterActions[i].Length > 1)
                        {
                            fighters[i].Walk(fighterActions[i][1]);
                        }
                        else
                        {
                            fighters[i].Walk("");
                        }
                        break;

                    case "jump":
                        if (fighterActions[i].Length > 1)
                        {
                            fighters[i].Jump(fighterActions[i][1]);
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
                        if (fighterActions[i].Length > 1)
                        {
                            fighters[i].Dash(fighterActions[i][1]);
                        }
                        else
                        {
                            fighters[i].Dash("");
                        }
                        break;

                    case "stand":
                        fighters[i].Stand();
                        break;

                    default:
                        Debug.LogError("Invalid action for Fighter " + (i+1) + "! " + actions[i]);
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
