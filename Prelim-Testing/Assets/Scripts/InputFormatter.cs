using LLMUnity;
using LLMUnitySamples;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;

public class InputFormatter : MonoBehaviour
{
    [NonSerialized]
    public string Prompt;

    /// <summary>
    /// Called when a user action is detected. It generates a prompt in the following format:
    /// "The user {eventType} a {adjective 1}, {adjective 2} ... a {name} {location}. They have done this {noi} time(s)."
    /// </summary>
    /// <param name="obj"> Relevant object to action</param>
    /// <param name="interactionType">enum describing action type (fx. PickedUp / Threw / MovedTo)</param>
    public string format(GameObject obj, string interactionType)
    {
        Descriptors _descriptor = obj.GetComponent<Descriptors>();
        int noi = _descriptor.GetNoi();
        Dictionary<string, string> adjectives = _descriptor.GetDescriptors();

        /*
        Debug.Log("KEYLIST:");
        List<string> test = new List<string>(adjectives.Keys);
        foreach (var item in test)
        {
            Debug.Log(item);
        }
        */
        //string Name = "Apple";


        string Name = adjectives["name"];
        string location = adjectives["location"];

        // Deconstruct dictionary to adjective list
        List<string> adjList = new List<string>();

        List<string> keys = new List<string>(adjectives.Keys);
        foreach (var key in keys)
        {
            if (key == "name") continue;
            if (key == "location") continue;
            if (key == "type") continue;

            adjList.Add(adjectives[key]);
        }
        
        //TODO: Figure out how to unpack the interraction type.

        string Prompt = ConstructPrompt(Name, location, adjList, interactionType, noi);
        Debug.Log($"Prompt: {Prompt}");

        return Prompt;
    }




    //Take all the components extracted from the descriptors dictionary and format a comprehensable prompt. 
    public string ConstructPrompt(string name, string location, List<string> adjList, string interactionType, int noi)
    {
        Debug.Log($"The object has: {adjList.Count} adjectives");
        
        string Prompt = "The user " + interactionType + " a ";

        int appendCount = 0;
        foreach (string adj in adjList)
        {
            Prompt += adj;
            if (appendCount != adjList.Count - 1)
            {
                Prompt += ", ";
            }
            appendCount++;
        }

        Prompt += " " + name + " located, " + location + ". ";

        if (noi > 1)
        {
            Prompt += "They have done this " + noi.ToString() + " time(s).";
        }

        return Prompt;
    }






    // Unpacking Functions -------------------------------------------------------- **Depricated**

    /*
    public Dictionary<string, string> unpackDictionary(Descriptors adj)
    {
        return adj.GetDescriptors();
    }

    public int unpackNOI(Descriptors adj)
    {
        return adj.GetNoi();
    }
    */
   
}
