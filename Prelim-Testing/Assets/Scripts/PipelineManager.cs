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

    private void Start()
    {
        if(testTargetObject != null)
        {
            trigger(testTargetObject, "picked up");
        }
    }

    public void trigger(GameObject obj, string ActionType)
    {
        
        if (obj == lastTargetObject) { return; }
        Debug.Log("Message sent");
        if (LLMGenerating) {CancelRequests();} // If the LLM has not completed a previous generation cancle it before starting a new one.
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
        }

    }

    // Append to starting prompt:
    // "AFTER EVERY MESSAGE SAY THE WORD END, Otherwise i will not be able to repond".
    public void ReplyClient(string text)
    {
        if (text.Contains("END"))
        {
            text = text.Substring(0, text.Length - 3);
            Reply(text);
        }

        if (text == "") return;

        if(text != lastServerText)
        {
            lastServerText = text;
            lastTextUpdateTime = Time.time;
            return;
        }

        if (Time.time - lastTextUpdateTime >= ServerUpdateThreshold)
        {
            Reply(text);
            lastServerText = "";
        }
    }


    public void Reply(string text)
    {
        LLMGenerating = false;
        Debug.Log($"AI: {text}");
        tts.StartGeneration(text, startTime); // Call the TTS Model with the reply frokm the LLM
        //TTSGenerating = true;
    }

    public void CancelRequests()
    {
        Debug.Log("REQUEST CANCLED!");
        llm.CancelRequests();
        //TODO: ADD REQUEST CANCLE TO ELEVEN LABS TTS SCRIPT
    }
}
