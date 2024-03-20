using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

// This class is responsible for converting text to speech.
public class TextToSpeech : MonoBehaviour
{
    // The ID of the voice to be used for speech synthesis.
    public string voiceID = "21m00Tcm4TlvDq8ikWAM";
    string url;
    // The API key for the Eleven Labs Text-to-Speech service.
    public string xiApiKey = "769852b4a6ecf3b3008813ef2d8355b7";
    // The ID of the model to be used for speech synthesis.
    public string modelId = "eleven_turbo_v2";
    public float stability = 0.5f;
    public float similarityBoost = 0.5f;
    public InputField inputField;

    private AudioSource audioSource;

    void Start()
    {
        // Construct the URL for the Eleven Labs Text-to-Speech service.
        url = "https://api.elevenlabs.io/v1/text-to-speech/" + voiceID + "/stream";

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // If the Return key is pressed, start the text-to-speech generation.
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(Generate(inputField.text));
        }
    }

    // This coroutine generates the speech from the input text.
    IEnumerator Generate(string inputText)
    {
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
        }
        else
        {
            // If the request fails, log the error.
            Debug.Log("WebException: " + www.error);
        }
    }
}
