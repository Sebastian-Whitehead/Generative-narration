using LLMUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;

public class InputFormatter : MonoBehaviour
{

    public enum eventType
    {
        PickedUp,
        Throw,
        MoveInto,
        MoveOutof,
        LookAt
    }

    public LLM llm;

    [NonSerialized]
    public string Prompt;


    // Start is called before the first frame update
    void Start()
    {
        trigger(this, eventType.PickedUp);   
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    /// <summary>
    /// Called when a user action is detected. It generates a prompt in the following format:
    /// "The user {eventType} a {adjective 1}, {adjective 2} ... a {name} {location}. They have done this {noi} time(s)."
    /// </summary>
    /// <param name="obj"> Relevant object to action</param>
    /// <param name="interactionType">enum describing action type (fx. PickedUp / Threw / MovedTo)</param>
    public void trigger(object obj, eventType interactionType)
    {
        string interractionType = "picked up";
        List<string> adjectives = new List<string>() { "red", "big" };
        string Name = "apple";
        string location = "from a small stall";
        int noi = 2;

        Prompt = "The user " + interractionType + " a ";

        int appendCount = 0;
        Debug.Log(adjectives.Count);
        foreach (string adj in adjectives)
        {
            Prompt += adj;
            if (appendCount != adjectives.Count - 1)
            {
                Prompt += ", ";
            }

            appendCount++;
        }

        Prompt += " " + Name + " " + location + ". ";

        if (noi > 1)
        {
            Prompt += "They have done this " + noi.ToString() + " time(s).";
        }

        Debug.Log(Prompt);
    }
}
