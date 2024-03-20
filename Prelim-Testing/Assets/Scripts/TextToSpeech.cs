using UnityEngine;
using UnityEngine.UI; // Add this line
using System.Collections;
using System.IO;
using System.Net;
using System;

public class TextToSpeech : MonoBehaviour
{
    const int CHUNK_SIZE = 1024;
    public string voiceID = "21m00Tcm4TlvDq8ikWAM";
    string url;
    public string xiApiKey = "769852b4a6ecf3b3008813ef2d8355b7";
    public string modelId = "eleven_turbo_v2";
    public float stability = 0.5f;
    public float similarityBoost = 0.5f;
    public InputField inputField; // Add this line

    void Start(){
        url = "https://api.elevenlabs.io/v1/text-to-speech/" + voiceID;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Return)) {
            StartCoroutine(Generate(inputField.text)); // Start the coroutine with the text from the input field
        }
    }

    IEnumerator Generate(string inputText) // Add a parameter to this method
    {
        // Create JSON data string
        string jsonData = "{\"text\":\"" + inputText + "\",\"model_id\":\"" + modelId + "\",\"voice_settings\":{\"stability\":" + stability.ToString(System.Globalization.CultureInfo.InvariantCulture) + ",\"similarity_boost\":" + similarityBoost.ToString(System.Globalization.CultureInfo.InvariantCulture) + "}}";

        // Set request headers
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "POST";
        request.Headers.Add("xi-api-key", xiApiKey);
        request.ContentType = "application/json";
        request.Accept = "audio/wav";

        // Write JSON data to request stream
        using (var streamWriter = new StreamWriter(request.GetRequestStream()))
        {
            streamWriter.Write(jsonData);
            streamWriter.Flush();
            streamWriter.Close();
        }

        // Get response
        try
        {
            WebResponse response = request.GetResponse();
            using (var stream = response.GetResponseStream())
            {
                // Save the stream to a file
                string path = Path.Combine(Application.dataPath, "Voices");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                using (var fileStream = new FileStream(Path.Combine(path, "output.wav"), FileMode.Create))
                {
                    byte[] buffer = new byte[CHUNK_SIZE];
                    int bytesRead;
                    while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        fileStream.Write(buffer, 0, bytesRead);
                    }
                }
            }
        }
        catch (WebException e)
        {
            Debug.Log("WebException: " + e.Message);
            if (e.Response != null)
            {
                using (var errorResponse = (HttpWebResponse)e.Response)
                {
                    using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                    {
                        string error = reader.ReadToEnd();
                        Debug.Log("Response Error: " + error);
                    }
                }
            }
        }
        yield return null;
    }
}
