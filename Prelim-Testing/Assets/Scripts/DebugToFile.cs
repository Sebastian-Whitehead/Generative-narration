using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Schema;
using System;

public class DebugToFile : MonoBehaviour
{
    public string definedFileName;
    private string Debugfilename = "";
    private string CSVfilename = "";

    // Saved Values
    private int s_interactCount = 0;
    private float s_LLMtime, s_TTStime, s_totalTime = 0.0f;
    private float frameRate;

     // public StreamWriter tw_csv;


    private void OnEnable()
    {
        Application.logMessageReceived += D_Log;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= D_Log;
    }
    


    private void Awake()
    {
        var curTime = System.DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss");
        CSVfilename = Application.dataPath + "/CSV_" + definedFileName + curTime + ".csv";
        Debugfilename = Application.dataPath + "/Debug_" + definedFileName + curTime + ".csv";
    }

    public void D_Log(string logString, string stackTrace, LogType type) // Used when you subscribe to Debug.log event
    {
        TextWriter tw = new StreamWriter(Debugfilename, true);
        tw.WriteLine("[" + System.DateTime.Now + "]" + logString);
        tw.Close();
    }

    public void C_Log(string logString)
    {
        TextWriter tw = new StreamWriter(CSVfilename, true);
        tw.WriteLine($"{logString}");
        tw.Close();
    }
    public void CSVLog(float LLMTime, float TTSTime, float totalTime, int interactCount)
    {
        // if any time values are 0 then they are all 0
        if (LLMTime == 0 || TTSTime == 0 || totalTime == 0) LLMTime = TTSTime = totalTime = 0; 
        s_LLMtime = LLMTime;
        s_TTStime = TTSTime;
        s_totalTime = totalTime;

        // update the saved count if it is updated
        if (interactCount != 0 && interactCount != s_interactCount) s_interactCount = interactCount; 
    }

    private void Start()
    {
        // Writes the header to the CSV file
        C_Log($"time,llmTime,ttsTime,genTime,interactCount,fps");
        Debug.Log("Header Printed");
        
    }

    private float timer;
    public float logInterval = 0.5f;
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > logInterval)
        {
            timer -= logInterval;
            frameRate = 1.0f / Time.deltaTime;

            TextWriter tw_csv = new StreamWriter(CSVfilename, true);
            tw_csv.WriteLine($"{Time.time},{s_LLMtime},{s_TTStime},{s_totalTime},{s_interactCount},{frameRate}");
            tw_csv.Close();

            s_LLMtime = s_TTStime = s_totalTime = 0f;
        }  
    }

    private void OnDestroy()
    {
        
    }
}

