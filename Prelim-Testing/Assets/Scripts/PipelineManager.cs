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
    public TextToSpeech tts;
    public InputFormatter formatter;

    public GameObject targetObject; // Should eventually be passed to it

    private bool IsGenerating;
    private float startTime;


    private void Start()
    {
        submitToLLM(formatter.format(targetObject, "picked up"));
    }

    public void trigger(GameObject obj, string ActionType)
    {
        submitToLLM(formatter.format(obj, ActionType));
    }
    // LLM Interface ----------------------------------------------
    void submitToLLM(string message)
    {
        IsGenerating = true;
        Debug.Log("AI: ...");

        startTime = Time.time;
        _ = llm.Chat(message, Reply);
    }

    public void Reply(string text)
    {
        Debug.Log($"AI: {text}");
        tts.StartGeneration(text, startTime); // Call the TTS Model with the reply frokm the LLM
    }

    public void CancelRequests()
    {
        Debug.Log("REQUEST CANCLED!");
        llm.CancelRequests();
        //TODO: ADD REQUEST CANCLE TO ELEVEN LABS TTS SCRIPT
    }
}
