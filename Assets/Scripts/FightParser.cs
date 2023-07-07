using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FightParser : MonoBehaviour
{
    private FighterController[] fighters;
    private string[] script;
    [Range(0f, 3f)][SerializeField] private float delay = 1f;
    private int index = 0;
    public bool running = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void StartFight(int levelNumber)
    {
        fighters = FindObjectsOfType<FighterController>();
        string file = "L" + levelNumber + ".txt";
        script = File.ReadAllLines(Path.Combine(Application.streamingAssetsPath + @"\FightScript\", file));
        index = 0;
        running = true;
        StartCoroutine(Fight());
    }

    private IEnumerator Fight() {
        // Filter out comments + whitespace
        while (index < script.Length && (script[index].Trim().StartsWith("#") || String.IsNullOrWhiteSpace(script[index])))
            index++;
        
        while (running)
        {
            // Make sure we didn't scroll too far
            if (index < script.Length)
            {
                // Gather commands into parsable lists
                string[] actions = script[index].ToLower().Trim().Split(",");
                string[][] fighterActions = { actions[0].Split(" "), actions[1].Split(" ") };

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
                        Debug.LogError("Too many inputs for Fighter " + (i + 1) + "! " + actions[i]);
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

                        case "wait":
                            break;

                        default:
                            Debug.LogError("Invalid action for Fighter " + (i + 1) + "! " + actions[i]);
                            break;
                    }
                }

                // Increment index for next run
                index++;

                // Filter out comments + whitespace
                while (index < script.Length && (script[index].Trim().StartsWith("#") || String.IsNullOrWhiteSpace(script[index])))
                    index++;
                
                if (index < script.Length)
                    yield return new WaitForSeconds(delay);
                else
                {
                    running = false;
                    Debug.Log("finished fight");
                    yield return null;
                }
            }
            else
            {
                running = false;
                Debug.Log("finished fight");
                yield return null;
            }
        }
    }
}
