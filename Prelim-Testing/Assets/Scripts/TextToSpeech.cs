using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using System;

[RequireComponent(typeof(AudioSource))]
public class TextToSpeech : MonoBehaviour
{
    // The names of the voices to be used for speech synthesis.
    public string[] voiceNames = {"Rachel", "Dorothy", "Michael", "George"};
    // The IDs of the voices to be used for speech synthesis.
    private string[] voiceIDs = {"21m00Tcm4TlvDq8ikWAM", "ThT5KcBeYPX3keUQqHPh", "flq6f7yk4E4fJM5XTYuZ", "JBFqnCBsd6RMkjVDRZzb"};
    public int selectedVoice = 0; // Default to the first voice
    string url;
    // The API key for the Eleven Labs Text-to-Speech service.
    public string xiApiKey = "769852b4a6ecf3b3008813ef2d8355b7";
    // The ID of the model to be used for speech synthesis.
    public string modelId = "eleven_turbo_v2";
    public float stability = 0.5f;
    public float similarityBoost = 0.5f;

    public PipelineManager pipelineManager;

    [NonSerialized]
    public  AudioSource audioSource;
    private float startTimeLLM;
    private float TimeTTS;

    [NonSerialized]
    public bool isGenerating = false;

    public DebugToFile logger;

    void Start()
    {
        UpdateUrl();
        audioSource = GetComponent<AudioSource>();

        if (logger == null) { 
        logger = FindObjectOfType<DebugToFile>();
        }
    }

    void Update()
    {
        // If the Return key is pressed, start the text-to-speech generation.
        /*
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(Generate(inputField.text));
        }
        */

        // If audio is generating, calculate the elapsed time and display it.
    }

    void UpdateUrl()
    {
        // Construct the URL for the Eleven Labs Text-to-Speech service.
        url = "https://api.elevenlabs.io/v1/text-to-speech/" + voiceIDs[selectedVoice] + "/stream";
    }

    public void StartGeneration(string TTS_text, float passed_time)
    {
        if (isGenerating || audioSource.isPlaying) { return; }
        isGenerating = true;
        StartCoroutine(Generate(TTS_text));
        startTimeLLM = passed_time;
    }

    private void Log_ResponseTime()
    {
        float Total_elapsedTime = Time.time - startTimeLLM;
        float TTS_elapsedTime = Time.time - TimeTTS;
        float LLM_elapsedTime = TimeTTS - startTimeLLM;

        pipelineManager.HasEnded = false;
        Debug.Log($"Total Time: {Total_elapsedTime} || TTS Time: {TTS_elapsedTime}  || LLM Time: {LLM_elapsedTime}");
        logger.CSVLog(LLM_elapsedTime, TTS_elapsedTime, Total_elapsedTime, 0);
    }

    // This coroutine generates the speech from the input text.
    IEnumerator Generate(string inputText)
    {
        UpdateUrl();
        // Reset the start time when a new audio generation request is made.
        TimeTTS = Time.time;
        

        // Construct the JSON data for the POST request.
        string jsonData = "{\"text\":\"" + inputText + "\",\"model_id\":\"" + modelId + "\",\"voice_settings\":{\"stability\":" + stability.ToString(System.Globalization.CultureInfo.InvariantCulture) + ",\"similarity_boost\":" + similarityBoost.ToString(System.Globalization.CultureInfo.InvariantCulture) + "}}";

        // Create a UnityWebRequest for getting an AudioClip.
        UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG);
        www.method = "POST";
        // Set the headers for the request.
        www.SetRequestHeader("xi-api-key", xiApiKey);
        www.SetRequestHeader("Content-Type", "application/json");
        // Set the upload handler for the request.
        www.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonData));

        // Send the request and yield until it is done.
        yield return www.SendWebRequest();

        // If the request is successful, play the synthesized speech.
        if (www.result == UnityWebRequest.Result.Success)
        {
            AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
            audioSource.clip = clip;
            audioSource.Play();
            isGenerating = false;

            //Log_ResponseTime();

            float Total_elapsedTime = Time.time - startTimeLLM;
            float TTS_elapsedTime = Time.time - TimeTTS;
            float LLM_elapsedTime = TimeTTS - startTimeLLM;

            pipelineManager.HasEnded = false;
            Debug.Log($"Total Time: {Total_elapsedTime} || TTS Time: {TTS_elapsedTime}  || LLM Time: {LLM_elapsedTime}");
            logger.CSVLog(LLM_elapsedTime, TTS_elapsedTime, Total_elapsedTime, 0);
        }
        else
        {
            isGenerating = false;
            Log_ResponseTime();
            // If the request fails, log the error.
            Debug.Log("WebException: " + www.error);
        }
    }
}
