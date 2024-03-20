using UnityEngine;
using UnityEngine.UI;

public class Button1 : MonoBehaviour
{
    // Declare a public AudioClip variable
    public UnityEngine.UI.Button yourButton;
    public AudioClip audioClip;

    private AudioSource audioSource;
    void Start()
    {
        // Check if the audio clip is assigned
        if (audioClip == null)
        {
            Debug.LogError("No audio clip assigned!");
            return;
        }

        // Add a listener to the button to play the audio clip when clicked
        yourButton.onClick.AddListener(PlayAudio);
    }

    void PlayAudio()
    {
                // Create an AudioSource component if it doesn't exist
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Assign the AudioClip from the inspector to the AudioSource
        audioSource.clip = audioClip;
        // Play the audio clip
        audioSource.Play();
    }
}

