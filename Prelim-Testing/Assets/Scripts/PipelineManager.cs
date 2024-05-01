using LLMUnity;
using LLMUnitySamples;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;

public class PipelineManager : MonoBehaviour
{

    public LLM llm;
    public LLMClient llmClient;
    public bool runRemote;

    public TextToSpeech tts;
    public InputFormatter formatter;

    public GameObject testTargetObject; // Should eventually be passed to it
    private GameObject lastTargetObject;

    [NonSerialized]
    public bool LLMGenerating;
    private float startTime;

    public float ServerUpdateThreshold = 500;
    private string lastServerText;
    private float lastTextUpdateTime;

    private float programStartTime;
    private DebugToFile logger;

    private int interractionCounter = 0;

    private void Start()
    {
        if(testTargetObject != null)
        {
            trigger(testTargetObject, "picked up");
        }

        logger = FindObjectOfType<DebugToFile>();
        programStartTime = Time.time;
    }

    public void trigger(GameObject obj, string ActionType)
    {
        if (obj == lastTargetObject) { return; }
        interractionCounter++;
        logger.CSVLog(0, 0, 0, interractionCounter);

        if (LLMGenerating) {CancelRequests();} // If the LLM has not completed a previous generation cancle it before starting a new one.
        if (tts.isGenerating || tts.audioSource.isPlaying) { return; }
        lastTargetObject = obj;
        submitToLLM(formatter.format(obj, ActionType));
    }
    // LLM Interface ----------------------------------------------
    void submitToLLM(string message)
    {
        LLMGenerating = true;
        Debug.Log("AI: ...");

        startTime = Time.time;

        if (!runRemote) { 
            _ = llm.Chat(message, Reply);
        }
        else { 
            _ = llmClient.Chat(message, ReplyClient);
            //_ = llmClient.Chat(message, Reply, ReplyCompleteClient);
        }

    }

    [NonSerialized]
    public bool HasEnded;
   // This is not a perfect solution as it can messup sometimes HOWEVER IT IS AS FAST AS PHYSICALL POSSIBLE.
   public void ReplyClient(string text)
   {

       Debug.Log(text);  
       if (text.Contains("END") && !HasEnded)
       {
           Debug.Log("Message End Detected");
           text = text.Substring(0, text.Length - 4);
           Reply(text);
       }

       // Backup
       if(text != lastServerText)
       {
           lastServerText = text;
           lastTextUpdateTime = Time.time;
           return;
       }
       if (lastServerText == "") return;
       if (Time.time - lastTextUpdateTime >= ServerUpdateThreshold)
       {
           Debug.Log("Timed Out!");
           lastServerText = "";
           Reply(text);
       }
    }
    
    public void Reply(string text)
    {
        LLMGenerating = false;
        Debug.Log($"AI: {text}");
        if (!tts.audioSource.isPlaying)
        {
            tts.StartGeneration(text, startTime); // Call the TTS Model with the reply frokm the LLM
        }
    }


    public void ReplyCompleteClient()
    {
        Debug.Log("Server AI Reply Complete");
    }

    public void CancelRequests()
    {
        Debug.Log("REQUEST CANCLED!");
        llm.CancelRequests();
        //TODO: ADD REQUEST CANCLE TO ELEVEN LABS TTS SCRIPT
    }

    private void OnApplicationQuit()
    {
        logger.C_Log($"Total Run Time: {Time.time - programStartTime}");
    }
}
