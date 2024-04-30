using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Descriptors : MonoBehaviour
{
    public enum Type
    {
        Object,
        Location,
        People,
        Background,
        Action
    }

    private Dictionary<string, string> adjectives = new Dictionary<string, string>(){};
    
    public Type type;

    [SerializeField]
    private bool log = false;

    public string Name;
    public string Location;

    public List<string> descriptors = new List<string>();

    public GroupedNoi noiGroup;
    public int noi = 0;
    private int adjectiveCount = 0;

    private void Awake()
    {
        adjectives.Add("name", Name);
        adjectives.Add("location", Location);
        adjectives.Add("type", type.ToString());
        
        foreach (string adj in descriptors)
        {
            adjectives.Add($"adj{adjectiveCount}", adj);
            adjectiveCount++;
        }

        conditionalLog(log, $"{type} {Name} has {adjectives.Count} descriptors");

        if (noiGroup != null) { 
        noi = noiGroup.TotalNoi;
        }
    }

    private void conditionalLog(bool condition, string text)
    {
        if (condition)
        { 
            Debug.Log(text);
        }
    }

    

    // --------------------------- Getters and Setters -----------------------------------
    public Dictionary<string, string> GetDescriptors()
    {
        UpdateNoi();
        return adjectives; 
    }
   

    public string GetDescriptor(string key)
    {
        UpdateNoi();
        return adjectives[key];
    }

    public int GetNoi()
    {
        return noi;
    }

    private void UpdateNoi()
    {
        if(noiGroup == null) 
        { 
            noi++; 
           
        } else
        {
            noiGroup.TotalNoi++;
            noi = noiGroup.TotalNoi;
        }

    }

    public void AddDescriptors(string key, string value)
    {
        descriptors.Add(value);
        adjectives.Add(key, value);
    }

    public void ModifyDescriptor(string key, string value)
    {
        adjectives[key] = value;
    }
}
